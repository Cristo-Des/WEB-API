CREATE DATABASE Proyecto;

USE Proyecto;

--------TABLAS-------

CREATE TABLE Roles (
    RolID INT IDENTITY(1,1) PRIMARY KEY,
    NombreRol VARCHAR(50) NOT NULL,
	Estatus BIT DEFAULT 1
);

CREATE TABLE Usuario (
    UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
	Nombre VARCHAR(100) NOT NULL,
    Correo VARCHAR(256) UNIQUE NOT NULL,
    Contraseña VARCHAR(256) NOT NULL,
    FechaCreacion DATETIME DEFAULT GETDATE() NOT NULL,
    RolID INT NOT NULL,
	Estatus BIT DEFAULT 1
    FOREIGN KEY (RolID) REFERENCES Roles(RolID)
);

CREATE TABLE UsuarioRol (
    UsuarioRolID INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioID INT,
    RolID INT,
	Estatus BIT DEFAULT 1
    FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID),
    FOREIGN KEY (RolID) REFERENCES Roles(RolID)
);

CREATE TABLE Clientes (
    ClienteID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Correo VARCHAR(256) UNIQUE NOT NULL,
    Telefono VARCHAR(20) NOT NULL,
    Direccion VARCHAR(255)NOT NULL,
    FechaNacimiento DATETIME NOT NULL,
	UsuarioID INT NOT NULL,
	Estatus BIT DEFAULT 1
	FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID)
);

CREATE TABLE SolicitudesCambioContraseña (
    SolicitudID INT IDENTITY(1,1) PRIMARY KEY,
    Correo VARCHAR(256) NOT NULL,
    Token VARCHAR(256) UNIQUE NOT NULL,
	UsuarioID INT NOT NULL,
	Estatus BIT DEFAULT 1
	FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID)
);


----------Registros----------

INSERT INTO Roles (NombreRol) VALUES ('Admin'), ('Usuario');

INSERT INTO Usuario (Nombre, Correo, Contraseña, RolId)
VALUES 
    ('Administrador', 'admin@dominio.com', 'ContraseñaSegura123', 1),  -- RolId 1 corresponde a 'Admin'
    ('Usuario Regular', 'usuario@dominio.com', 'ContraseñaSegura456', 2); -- RolId 2 corresponde a 'Usuario'

INSERT INTO Clientes (Nombre, Correo, Telefono, Direccion, FechaNacimiento, UsuarioId)
VALUES 
    ('Juan Perez', 'Juan@gmail.com', '123456789', 'Calle Falsa 123, Ciudad',  '1980-01-01',1),  -- UsuarioId 1 corresponde a 'Administrador'
    ('Maria Gomez', 'Maria@gmail.com', '987654321', 'Calle Real 456, Ciudad',  '1990-02-02',2);  -- UsuarioId 2 corresponde a 'Usuario Regular'

INSERT INTO SolicitudesCambioContraseña (Correo, Token, UsuarioId)
VALUES 
    ('admin@dominio.com', 'abc123validaciontoken', 1),  -- UsuarioId 1
    ('usuario@dominio.com', 'xyz456validaciontoken', 2);  -- UsuarioId 2

INSERT INTO UsuarioRol (UsuarioId, RolId)
VALUES 
    (1, 1),  -- UsuarioId 1 (Administrador) tiene el rol de Admin (RolId 1)
    (2, 2);  -- UsuarioId 2 (Usuario Regular) tiene el rol de Usuario (RolId 2)

	Select* from  Roles
	Select* from  Usuario
	Select* from  Clientes
	Select* from  UsuarioRol
	Select* from  SolicitudesCambioContraseña

	DBCC CHECKIDENT (SolicitudesCambioContraseña, NORESEED);
	DELETE FROM SolicitudesCambioContraseña;
	DBCC CHECKIDENT (SolicitudesCambioContraseña, RESEED, 0);