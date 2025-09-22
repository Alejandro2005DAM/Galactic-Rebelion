<?php
header('Content-Type: application/json');

try {
    // Incluir el archivo de conexión
    include "dbConnection.php";
    
    // Verificar si se recibieron los datos necesarios
    if(!isset($_POST['userName']) || !isset($_POST['score'])) {
        echo json_encode(['done' => false, 'message' => 'Faltan datos requeridos']);
        exit();
    }
    
    $userName = $_POST['userName'];
    $score = intval($_POST['score']);
    $fecha = date('Y-m-d H:i:s'); // Fecha y hora actual
    
    // Verificar que el usuario exista
    $sqlCheck = "SELECT userName FROM usuarios WHERE userName = :userName";
    $stmtCheck = $pdo->prepare($sqlCheck);
    $stmtCheck->execute([':userName' => $userName]);
    
    if ($stmtCheck->rowCount() === 0) {
        echo json_encode(['done' => false, 'message' => 'Usuario no encontrado']);
        exit();
    }
    
    // Guardar la puntuación en la tabla partidas
    $sqlInsert = "INSERT INTO partidas (userName, puntos, fecha) VALUES (:userName, :puntos, :fecha)";
    $stmtInsert = $pdo->prepare($sqlInsert);
    $result = $stmtInsert->execute([
        ':userName' => $userName, 
        ':puntos' => $score, 
        ':fecha' => $fecha
    ]);
    
    if ($result) {
        echo json_encode(['done' => true, 'message' => 'Puntuación guardada con éxito']);
    } else {
        echo json_encode(['done' => false, 'message' => 'Error al guardar puntuación']);
    }

} catch (PDOException $e) {
    echo json_encode(['done' => false, 'message' => 'Error en el servidor: ' . $e->getMessage()]);
}
?>