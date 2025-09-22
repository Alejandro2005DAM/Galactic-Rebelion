<?php
// Establecer tipo de contenido para la respuesta
header('Content-Type: application/json');

try {
    // Incluir el archivo de conexión
    include "dbConnection.php";

    // Verificar si se recibieron los datos necesarios
    if (!isset($_POST['userName']) || !isset($_POST['puntos'])) {
        echo json_encode(['done' => false, 'message' => 'Faltan datos requeridos']);
        exit();
    }

    $userName = $_POST['userName'];
    $puntos = intval($_POST['puntos']); // Asegura que puntos sea un número entero

    // Verificar si el usuario existe
    $sqlCheck = "SELECT userName FROM usuarios WHERE userName = :userName";
    $stmtCheck = $pdo->prepare($sqlCheck);
    $stmtCheck->execute([':userName' => $userName]);

    if ($stmtCheck->rowCount() === 0) {
        echo json_encode(['done' => false, 'message' => 'El usuario no existe']);
        exit();
    }

    // Insertar la partida
    $sqlInsert = "INSERT INTO partidas (userName, puntos) VALUES (:userName, :puntos)";
    $stmtInsert = $pdo->prepare($sqlInsert);
    $result = $stmtInsert->execute([':userName' => $userName, ':puntos' => $puntos]);

    if ($result) {
        echo json_encode(['done' => true, 'message' => 'Partida registrada con éxito']);
    } else {
        echo json_encode(['done' => false, 'message' => 'Error al registrar la partida']);
    }

} catch (PDOException $e) {
    echo json_encode(['done' => false, 'message' => 'Error en el servidor: ' . $e->getMessage()]);
}
?>