<?php
header('Content-Type: application/json');

try {
    // Incluir el archivo de conexión
    include "dbConnection.php";
    
    // Obtener el límite de registros a mostrar (por defecto 10)
    $limit = isset($_POST['limit']) ? intval($_POST['limit']) : 10;
    
    // Asegurarse de que el límite es un número positivo
    if ($limit <= 0) {
        $limit = 10;
    }
    
    // Obtener las mejores puntuaciones de la tabla partidas
    // Usando una subconsulta para obtener la fecha del record más alto de cada usuario
    $sqlSelect = "SELECT p1.userName, p1.puntos as score, DATE_FORMAT(p1.fecha, '%Y-%m-%d %H:%i:%s') as date
                  FROM partidas p1
                  INNER JOIN (
                      SELECT userName, MAX(puntos) as max_puntos
                      FROM partidas
                      GROUP BY userName
                  ) p2 ON p1.userName = p2.userName AND p1.puntos = p2.max_puntos
                  GROUP BY p1.userName, p1.puntos, p1.fecha
                  ORDER BY score DESC 
                  LIMIT :limit";
    
    $stmtSelect = $pdo->prepare($sqlSelect);
    $stmtSelect->bindParam(':limit', $limit, PDO::PARAM_INT);
    $stmtSelect->execute();
    
    $scores = $stmtSelect->fetchAll(PDO::FETCH_ASSOC);
    
    echo json_encode([
        'done' => true,
        'message' => count($scores) > 0 ? 'Puntuaciones obtenidas con éxito' : 'No hay puntuaciones registradas',
        'scores' => $scores
    ]);

} catch (PDOException $e) {
    echo json_encode([
        'done' => false,
        'message' => 'Error en el servidor: ' . $e->getMessage(),
        'scores' => []
    ]);
}
?>