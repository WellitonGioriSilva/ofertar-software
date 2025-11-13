CREATE DATABASE db_oferta;
USE db_oferta;

CREATE TABLE User (
    id INT PRIMARY KEY,
    email VARCHAR(100),
    passwordHash VARCHAR(255),
    name VARCHAR(100),
    recoveryToken VARCHAR(255),
    UNIQUE (email, name)
);

CREATE TABLE Address (
    id INT PRIMARY KEY,
    street VARCHAR(100),
    number VARCHAR(10),
    zipCode VARCHAR(8),
    complement TEXT,
    neighborhood VARCHAR(100)
);

CREATE TABLE Profession (
    id INT PRIMARY KEY,
    code VARCHAR(10),
    name VARCHAR(100)
);

CREATE TABLE Tither (
    id INT PRIMARY KEY,
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
    FOREIGN KEY (spouse_id) REFERENCES Tither (id) ON DELETE SET NULL,
    FOREIGN KEY (profession_id) REFERENCES Profession (id) ON DELETE SET NULL,
    FOREIGN KEY (address_id) REFERENCES Address (id) ON DELETE RESTRICT
);

CREATE TABLE Tithe (
    id INT PRIMARY KEY,
    offeringDate DATE,
    amount DOUBLE,
    paymentMethod CHAR(1),
    tither_id INT,
    user_id INT,
    FOREIGN KEY (tither_id) REFERENCES Tither (id) ON DELETE CASCADE,
    FOREIGN KEY (user_id) REFERENCES User (id) ON DELETE CASCADE
);
