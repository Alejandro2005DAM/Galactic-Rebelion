<?php
include "dbConnection.php";

// Obtener datos del POST
$userName = $_POST['userName'];
$pass = hash("sha256", $_POST['pass']);

// Prevenir inyección SQL usando prepared statements
$sql = "SELECT userName FROM usuarios WHERE userName = :userName AND password = :pass";
$stmt = $pdo->prepare($sql);
$stmt->bindParam(':userName', $userName);
$stmt->bindParam(':pass', $pass);
$stmt->execute();

// Verificar resultados
if($stmt->rowCount() > 0) {
    $data = array(
        'done' => true, 
        'message' => "Bienvenido $userName" // Corregido: $userName en lugar de $username
    );
    header('Content-Type: application/json'); // Corregido: Content-Type
    echo json_encode($data);
    exit();
} else {
    $data = array(
        'done' => false, 
        'message' => "Error: nombre de usuario o contraseña incorrectos"
    );
    header('Content-Type: application/json'); // Corregido: Content-Type
    echo json_encode($data);
    exit();
}
?>