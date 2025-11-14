CREATE DATABASE db_oferta;
USE db_oferta;

CREATE TABLE Role (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(50)
);

CREATE TABLE Church (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100),
    isActive BOOLEAN
);

CREATE TABLE User (
    id INT PRIMARY KEY AUTO_INCREMENT,
    email VARCHAR(100),
    passwordHash VARCHAR(255),
    name VARCHAR(100),
    recoveryToken VARCHAR(255),
    church_id INT,
    FOREIGN KEY (church_id) REFERENCES Church (id) ON DELETE SET NULL,
    UNIQUE (email, name)
);

CREATE TABLE UserRole (
    id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT,
    role_id INT,
    FOREIGN KEY (user_id) REFERENCES User (id) ON DELETE CASCADE,
    FOREIGN KEY (role_id) REFERENCES Role (id) ON DELETE CASCADE
);

CREATE TABLE Address (
    id INT PRIMARY KEY AUTO_INCREMENT,
    street VARCHAR(100),
    number VARCHAR(10),
    zipCode VARCHAR(8),
    complement TEXT,
    neighborhood VARCHAR(100)
);

CREATE TABLE Profession (
    id INT PRIMARY KEY AUTO_INCREMENT,
    code VARCHAR(10),
    name VARCHAR(100)
);

CREATE TABLE Tither (
    id INT PRIMARY KEY AUTO_INCREMENT,
    phone VARCHAR(11),
    email VARCHAR(100),
    birthDate DATE,
    maritalStatus CHAR(1),
    isActive BOOLEAN,
    company VARCHAR(100),
    name VARCHAR(100),
    spouse_id INT,
    profession_id INT,
    address_id INT,
    church_id INT,
    FOREIGN KEY (spouse_id) REFERENCES Tither (id) ON DELETE SET NULL,
    FOREIGN KEY (profession_id) REFERENCES Profession (id) ON DELETE SET NULL,
    FOREIGN KEY (address_id) REFERENCES Address (id) ON DELETE RESTRICT,
    FOREIGN KEY (church_id) REFERENCES Church (id) ON DELETE RESTRICT
);

CREATE TABLE Tithe (
    id INT PRIMARY KEY AUTO_INCREMENT,
    offeringDate DATE,
    amount DOUBLE,
    paymentMethod CHAR(1),
    tither_id INT,
    user_id INT,
    FOREIGN KEY (tither_id) REFERENCES Tither (id) ON DELETE CASCADE,
    FOREIGN KEY (user_id) REFERENCES User (id) ON DELETE CASCADE
);
