<?php
// Establecer tipo de contenido para la respuesta
header('Content-Type: application/json');

try {
    // Incluir el archivo de conexión
    include "dbConnection.php";
    
    // Verificar si se recibieron los datos necesarios
    if(!isset($_POST['userName']) || !isset($_POST['email']) || !isset($_POST['pass'])) {
        echo json_encode(['done' => false, 'message' => 'Faltan datos requeridos']);
        exit();
    }
    
    $userName = $_POST['userName'];
    $email = $_POST['email'];
    $pass = hash('sha256', $_POST['pass']);

    // Verificar si el usuario o email ya existen
    $sqlCheck = "SELECT userName FROM usuarios WHERE userName = :userName OR email = :email";
    $stmtCheck = $pdo->prepare($sqlCheck);
    $stmtCheck->execute([':userName' => $userName, ':email' => $email]);

    if ($stmtCheck->rowCount() > 0) {
        echo json_encode(['done' => false, 'message' => 'Usuario o email ya existen']);
        exit();
    }

    // Crear usuario
    $sqlInsert = "INSERT INTO usuarios (userName, email, password) VALUES (:userName, :email, :pass)";
    $stmtInsert = $pdo->prepare($sqlInsert);
    $result = $stmtInsert->execute([':userName' => $userName, ':email' => $email, ':pass' => $pass]);
    
    if ($result) {
        echo json_encode(['done' => true, 'message' => 'Usuario creado con éxito']);
    } else {
        echo json_encode(['done' => false, 'message' => 'Error al crear usuario']);
    }

} catch (PDOException $e) {
    echo json_encode(['done' => false, 'message' => 'Error en el servidor: ' . $e->getMessage()]);
}
?>