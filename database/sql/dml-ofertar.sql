USE db_oferta;

INSERT INTO Role (name) VALUES("ADMIN"), ("USUÁRIO");
INSERT INTO Church (name, isActive) VALUES("IGREJA ADMIN", true), ("COMUNIDADE NOSSA SENHORA DAS GRAÇAS", true);
INSERT INTO User (email, passwordHash, name, church_id) VALUES ("gioriwelliton47@gmail.com", "$2a$12$RXNuii3SD1nc1TeOIB5/3uyMoi/oA8EpqL1xc02VeHFOJrDH07Ut.", "Giori", 1);

SELECT * FROM User;
SELECT * FROM Church;