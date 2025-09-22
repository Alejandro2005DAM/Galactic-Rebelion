<?php
// dbConnection.php - Solo establece la conexión, sin enviar respuestas

try {
    // Conexión a la base de datos
    $pdo = new PDO('mysql:host=localhost;dbname=game', 'root', '');
    $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    $pdo->exec('SET NAMES utf8');
    
    // No incluimos un exit() aquí para permitir que otros scripts utilicen esta conexión
    
} catch (PDOException $e) {
    // Solo lanzamos la excepción sin terminar la ejecución
    throw new PDOException("Error de conexión: " . $e->getMessage());
}
?>