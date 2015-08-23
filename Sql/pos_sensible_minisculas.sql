/*
SQLyog Ultimate v9.02 
MySQL - 5.5.0-m2-community : Database - pos
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`pos` /*!40100 DEFAULT CHARACTER SET utf8 COLLATE utf8_bin */;

USE `pos`;

/*Table structure for table `Articulos` */

DROP TABLE IF EXISTS `Articulos`;

CREATE TABLE `Articulos` (
  `IdArticuloART` varchar(50) NOT NULL,
  `IdItemART` int(11) DEFAULT NULL,
  `IdColorART` int(11) DEFAULT NULL,
  `TalleART` varchar(2) DEFAULT NULL,
  `IdProveedorART` int(11) DEFAULT NULL,
  `DescripcionART` varchar(55) DEFAULT NULL,
  `DescripcionWebART` varchar(50) DEFAULT NULL,
  `PrecioCostoART` decimal(19,0) DEFAULT NULL,
  `PrecioPublicoART` decimal(19,0) DEFAULT NULL,
  `PrecioMayorART` decimal(19,0) DEFAULT NULL,
  `FechaART` datetime DEFAULT NULL,
  `ImagenART` varchar(20) DEFAULT NULL,
  `ImagenBackART` varchar(20) DEFAULT NULL,
  `ImagenColorART` varchar(20) DEFAULT NULL,
  `ActivoWebART` int(1) DEFAULT NULL,
  `NuevoART` int(1) DEFAULT NULL,
  PRIMARY KEY (`IdArticuloART`),
  KEY `FK_Color` (`IdColorART`),
  KEY `FK_Proveedor` (`IdProveedorART`),
  KEY `FK_Item` (`IdItemART`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `Clientes` */

DROP TABLE IF EXISTS `Clientes`;

CREATE TABLE `Clientes` (
  `IdClienteCLI` int(11) NOT NULL,
  `RazonSocialCLI` varchar(50) DEFAULT NULL,
  `CUIT` varchar(50) DEFAULT NULL,
  `DireccionCLI` varchar(50) DEFAULT NULL,
  `LocalidadCLI` varchar(50) DEFAULT NULL,
  `ProvinciaCLI` varchar(50) DEFAULT NULL,
  `TransporteCLI` varchar(50) DEFAULT NULL,
  `ContactoCLI` varchar(50) DEFAULT NULL,
  `TelefonoCLI` varchar(50) DEFAULT NULL,
  `MovilCLI` varchar(50) DEFAULT NULL,
  `CorreoCLI` varchar(60) DEFAULT NULL,
  `FechaNacCLI` datetime DEFAULT NULL,
  PRIMARY KEY (`IdClienteCLI`),
  KEY `IdClienteCLI` (`IdClienteCLI`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `FondoCaja` */

DROP TABLE IF EXISTS `FondoCaja`;

CREATE TABLE `FondoCaja` (
  `IdFONP` int(11) DEFAULT NULL,
  `FechaFONP` datetime NOT NULL,
  `IdPcFONP` int(11) NOT NULL,
  `ImporteFONP` double DEFAULT NULL,
  PRIMARY KEY (`FechaFONP`,`IdPcFONP`),
  KEY `FK_FondoCaja` (`IdPcFONP`),
  CONSTRAINT `FK_FondoCaja` FOREIGN KEY (`IdPcFONP`) REFERENCES `pc` (`IdPC`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `FormasPago` */

DROP TABLE IF EXISTS `FormasPago`;

CREATE TABLE `FormasPago` (
  `IdFormaPagoFOR` int(11) NOT NULL,
  `DescripcionFOR` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`IdFormaPagoFOR`),
  KEY `IdFormaPagoFOR` (`IdFormaPagoFOR`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `Locales` */

DROP TABLE IF EXISTS `Locales`;

CREATE TABLE `Locales` (
  `IdLocalLOC` int(11) NOT NULL AUTO_INCREMENT,
  `NombreLOC` varchar(50) DEFAULT NULL,
  `DireccionLOC` varchar(50) DEFAULT NULL,
  `TelefonoLOC` varchar(50) DEFAULT NULL,
  `ActivoWebLOC` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`IdLocalLOC`),
  UNIQUE KEY `NombreLOC` (`NombreLOC`),
  KEY `IdLocalLOC` (`IdLocalLOC`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;

/*Table structure for table `Pc` */

DROP TABLE IF EXISTS `Pc`;

CREATE TABLE `Pc` (
  `IdPC` int(11) NOT NULL,
  `IdLocalPC` int(11) DEFAULT NULL,
  `Detalle` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`IdPC`),
  KEY `IdLocalPC` (`IdLocalPC`),
  KEY `IdPC` (`IdPC`),
  CONSTRAINT `FK_Pc` FOREIGN KEY (`IdLocalPC`) REFERENCES `locales` (`IdLocalLOC`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `Proveedores` */

DROP TABLE IF EXISTS `Proveedores`;

CREATE TABLE `Proveedores` (
  `IdProveedorPRO` int(11) NOT NULL,
  `RazonSocialPRO` varchar(50) DEFAULT NULL,
  `DireccionPRO` varchar(50) DEFAULT NULL,
  `CodigoPostalPRO` varchar(50) DEFAULT NULL,
  `TelefonoPRO` varchar(50) DEFAULT NULL,
  `ContactoPRO` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`IdProveedorPRO`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `Stock` */

DROP TABLE IF EXISTS `Stock`;

CREATE TABLE `Stock` (
  `IdArticuloSTK` varchar(50) NOT NULL,
  `IdLocalSTK` int(11) NOT NULL,
  `CantidadSTK` int(11) DEFAULT NULL,
  PRIMARY KEY (`IdArticuloSTK`,`IdLocalSTK`),
  KEY `FK_Stock_Local` (`IdLocalSTK`),
  CONSTRAINT `FK_Stock_Articulos` FOREIGN KEY (`IdArticuloSTK`) REFERENCES `articulos` (`IdArticuloART`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_Stock_Local` FOREIGN KEY (`IdLocalSTK`) REFERENCES `locales` (`IdLocalLOC`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `StockMov` */

DROP TABLE IF EXISTS `StockMov`;

CREATE TABLE `StockMov` (
  `IdMovMSTK` int(11) NOT NULL,
  `FechaMSTK` datetime DEFAULT NULL,
  `OrigenMSTK` int(11) DEFAULT NULL,
  `DestinoMSTK` int(11) DEFAULT NULL,
  `CompensaMSTK` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`IdMovMSTK`),
  KEY `FK_StockMov_Origen` (`OrigenMSTK`),
  KEY `FK_StockMov_Destino` (`DestinoMSTK`),
  CONSTRAINT `FK_StockMov_Destino` FOREIGN KEY (`DestinoMSTK`) REFERENCES `locales` (`IdLocalLOC`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_StockMov_Origen` FOREIGN KEY (`OrigenMSTK`) REFERENCES `locales` (`IdLocalLOC`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `StockMovDetalle` */

DROP TABLE IF EXISTS `StockMovDetalle`;

CREATE TABLE `StockMovDetalle` (
  `IdMSTKD` int(11) NOT NULL,
  `IdMovMSTKD` int(11) DEFAULT NULL,
  `IdArticuloMSTKD` varchar(50) DEFAULT NULL,
  `CantidadMSTKD` int(11) DEFAULT NULL,
  `CompensaMSTKD` tinyint(1) DEFAULT '0',
  `OrigenMSTKD` tinyint(2) DEFAULT NULL,
  `DestinoMSTKD` tinyint(2) DEFAULT NULL,
  PRIMARY KEY (`IdMSTKD`),
  KEY `FK_StockMovDetalle` (`IdMovMSTKD`),
  KEY `FK_StockMovDetalle_Articulos` (`IdArticuloMSTKD`),
  CONSTRAINT `FK_StockMovDetalle` FOREIGN KEY (`IdMovMSTKD`) REFERENCES `stockmov` (`IdMovMSTK`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_StockMovDetalle_Articulos` FOREIGN KEY (`IdArticuloMSTKD`) REFERENCES `articulos` (`IdArticuloART`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `TesoreriaMovimentos` */

DROP TABLE IF EXISTS `TesoreriaMovimentos`;

CREATE TABLE `TesoreriaMovimentos` (
  `IdMovTESM` int(11) NOT NULL,
  `FechaTESM` datetime DEFAULT NULL,
  `IdPcTESM` int(11) DEFAULT NULL,
  `DetalleTESM` varchar(200) DEFAULT NULL,
  `ImporteTESM` double DEFAULT NULL,
  PRIMARY KEY (`IdMovTESM`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `Ventas` */

DROP TABLE IF EXISTS `Ventas`;

CREATE TABLE `Ventas` (
  `IdVentaVEN` int(11) NOT NULL,
  `IdPCVEN` int(11) DEFAULT NULL,
  `FechaVEN` datetime DEFAULT NULL,
  `IdClienteVEN` int(11) DEFAULT NULL,
  PRIMARY KEY (`IdVentaVEN`),
  KEY `FK_Ventas` (`IdClienteVEN`),
  KEY `FK_Ventas_Pc` (`IdPCVEN`),
  CONSTRAINT `FK_Ventas_Pc` FOREIGN KEY (`IdPCVEN`) REFERENCES `pc` (`IdPC`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `VentasDetalle` */

DROP TABLE IF EXISTS `VentasDetalle`;

CREATE TABLE `VentasDetalle` (
  `IdDVEN` int(11) NOT NULL,
  `IdVentaDVEN` int(11) DEFAULT NULL,
  `IdLocalDVEN` int(3) DEFAULT NULL,
  `IdArticuloDVEN` varchar(50) DEFAULT NULL,
  `DescripcionDVEN` varchar(50) DEFAULT NULL,
  `CantidadDVEN` int(11) DEFAULT NULL,
  `PrecioPublicoDVEN` double DEFAULT NULL,
  `PrecioCostoDVEN` double DEFAULT NULL,
  `PrecioMayorDVEN` double DEFAULT NULL,
  `IdFormaPagoDVEN` int(11) DEFAULT NULL,
  `NroCuponDVEN` int(11) DEFAULT NULL,
  `NroFacturaDVEN` int(11) DEFAULT NULL,
  `IdEmpleadoDVEN` int(11) DEFAULT NULL,
  `LiquidadoDVEN` bit(1) DEFAULT NULL,
  `EsperaDVEN` bit(1) DEFAULT NULL,
  `DevolucionDVEN` smallint(1) DEFAULT NULL,
  PRIMARY KEY (`IdDVEN`),
  KEY `FK_VentasDetalle_Forma` (`IdFormaPagoDVEN`),
  KEY `FK_VentasDetalle` (`IdVentaDVEN`),
  KEY `FK_VentasDetalle_Articulos` (`IdArticuloDVEN`),
  CONSTRAINT `FK_VentasDetalle` FOREIGN KEY (`IdVentaDVEN`) REFERENCES `ventas` (`IdVentaVEN`) ON DELETE CASCADE,
  CONSTRAINT `FK_VentasDetalle_Articulos` FOREIGN KEY (`IdArticuloDVEN`) REFERENCES `articulos` (`IdArticuloART`) ON DELETE SET NULL ON UPDATE SET NULL,
  CONSTRAINT `FK_VentasDetalle_Forma` FOREIGN KEY (`IdFormaPagoDVEN`) REFERENCES `formaspago` (`IdFormaPagoFOR`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `VentasDetalleFallidas` */

DROP TABLE IF EXISTS `VentasDetalleFallidas`;

CREATE TABLE `VentasDetalleFallidas` (
  `Id` int(11) DEFAULT NULL,
  `IdVenta` int(11) DEFAULT NULL,
  `Accion` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `VentasFallidas` */

DROP TABLE IF EXISTS `VentasFallidas`;

CREATE TABLE `VentasFallidas` (
  `Id` int(11) DEFAULT NULL,
  `Accion` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `fondocajafllds` */

DROP TABLE IF EXISTS `fondocajafllds`;

CREATE TABLE `fondocajafllds` (
  `FechaFONP` datetime NOT NULL,
  `IdPcFONP` int(11) NOT NULL,
  `ImporteFONP` double DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/* Trigger structure for table `StockMov` */

DELIMITER $$

/*!50003 DROP TRIGGER*//*!50032 IF EXISTS */ /*!50003 `stockMov_delete_cascade` */$$

/*!50003 CREATE */ /*!50017 DEFINER = 'ncsoftwa_re'@'%' */ /*!50003 TRIGGER `stockMov_delete_cascade` BEFORE DELETE ON `StockMov` FOR EACH ROW BEGIN
	DELETE FROM StockMovDetalle WHERE `IdMovMSTKD` = OLD.`IdMovMSTK`;
    END */$$


DELIMITER ;

/* Trigger structure for table `StockMovDetalle` */

DELIMITER $$

/*!50003 DROP TRIGGER*//*!50032 IF EXISTS */ /*!50003 `stockMov_insert_stock_updt` */$$

/*!50003 CREATE */ /*!50017 DEFINER = 'ncsoftwa_re'@'%' */ /*!50003 TRIGGER `stockMov_insert_stock_updt` AFTER INSERT ON `StockMovDetalle` FOR EACH ROW BEGIN
    DECLARE cantidad INT(11);
    SET cantidad = NEW.CantidadMSTKD;
	IF NEW.OrigenMSTKD = 1 THEN
		UPDATE Stock SET CantidadSTK = CantidadSTK + cantidad
		WHERE IdArticuloSTK = NEW.IdArticuloMSTKD AND IdLocalSTK = NEW.DestinoMSTKD;
	ELSEIF NEW.DestinoMSTKD = 2 THEN
		UPDATE Stock SET CantidadSTK = CantidadSTK - cantidad
		WHERE IdArticuloSTK = NEW.IdArticuloMSTKD AND IdLocalSTK = NEW.OrigenMSTKD;
	ELSE
		UPDATE Stock SET CantidadSTK = CantidadSTK + cantidad
		WHERE IdArticuloSTK = NEW.IdArticuloMSTKD AND IdLocalSTK = NEW.DestinoMSTKD;
		UPDATE Stock SET CantidadSTK = CantidadSTK - cantidad
		WHERE IdArticuloSTK = NEW.IdArticuloMSTKD AND IdLocalSTK = NEW.OrigenMSTKD;					
	END IF;
    END */$$


DELIMITER ;

/* Trigger structure for table `StockMovDetalle` */

DELIMITER $$

/*!50003 DROP TRIGGER*//*!50032 IF EXISTS */ /*!50003 `stockMov_edit_stock_updt` */$$

/*!50003 CREATE */ /*!50017 DEFINER = 'ncsoftwa_re'@'%' */ /*!50003 TRIGGER `stockMov_edit_stock_updt` AFTER UPDATE ON `StockMovDetalle` FOR EACH ROW BEGIN
    DECLARE cantidad INT(11);    
	IF OLD.`IdArticuloMSTKD` = NEW.IdArticuloMSTKD THEN
		SET cantidad = NEW.CantidadMSTKD - OLD.CantidadMSTKD;
		IF OLD.OrigenMSTKD = 1 THEN
			UPDATE Stock SET CantidadSTK = CantidadSTK + cantidad
				WHERE IdArticuloSTK = OLD.IdArticuloMSTKD AND IdLocalSTK = OLD.DestinoMSTKD;
		ELSEIF OLD.DestinoMSTKD = 2 THEN
			UPDATE Stock SET CantidadSTK = CantidadSTK + cantidad
				WHERE IdArticuloSTK = OLD.IdArticuloMSTKD AND IdLocalSTK = OLD.OrigenMSTKD;
		ELSE
			UPDATE Stock SET CantidadSTK = CantidadSTK + cantidad
				WHERE IdArticuloSTK = OLD.IdArticuloMSTKD AND IdLocalSTK = OLD.DestinoMSTKD;	
			UPDATE Stock SET CantidadSTK = CantidadSTK + cantidad
				WHERE IdArticuloSTK = OLD.IdArticuloMSTKD AND IdLocalSTK = OLD.OrigenMSTKD;
		END IF;
	ELSE
		IF OLD.OrigenMSTKD = 1 THEN
			UPDATE Stock SET CantidadSTK = CantidadSTK - OLD.CantidadMSTKD
				WHERE IdArticuloSTK = OLD.IdArticuloMSTKD AND IdLocalSTK = OLD.DestinoMSTKD;
			UPDATE Stock SET CantidadSTK = CantidadSTK + NEW.CantidadMSTKD
				WHERE IdArticuloSTK = NEW.IdArticuloMSTKD AND IdLocalSTK = NEW.DestinoMSTKD;				
		ELSEIF OLD.DestinoMSTKD = 2 THEN
			UPDATE Stock SET CantidadSTK = CantidadSTK + OLD.CantidadMSTKD
				WHERE IdArticuloSTK = OLD.IdArticuloMSTKD AND IdLocalSTK = OLD.OrigenMSTKD;
			UPDATE Stock SET CantidadSTK = CantidadSTK - NEW.CantidadMSTKD
				WHERE IdArticuloSTK = NEW.IdArticuloMSTKD AND IdLocalSTK = NEW.OrigenMSTKD;
		ELSE
			UPDATE Stock SET CantidadSTK = CantidadSTK - OLD.CantidadMSTKD
				WHERE IdArticuloSTK = OLD.IdArticuloMSTKD AND IdLocalSTK = OLD.DestinoMSTKD;
			UPDATE Stock SET CantidadSTK = CantidadSTK + NEW.CantidadMSTKD
				WHERE IdArticuloSTK = NEW.IdArticuloMSTKD AND IdLocalSTK = NEW.DestinoMSTKD;				
			UPDATE Stock SET CantidadSTK = CantidadSTK + OLD.CantidadMSTKD
				WHERE IdArticuloSTK = OLD.IdArticuloMSTKD AND IdLocalSTK = OLD.OrigenMSTKD;
			UPDATE Stock SET CantidadSTK = CantidadSTK - NEW.CantidadMSTKD
				WHERE IdArticuloSTK = NEW.IdArticuloMSTKD AND IdLocalSTK = NEW.OrigenMSTKD;
		END IF;			
	END IF;
    END */$$


DELIMITER ;

/* Trigger structure for table `StockMovDetalle` */

DELIMITER $$

/*!50003 DROP TRIGGER*//*!50032 IF EXISTS */ /*!50003 `stockMov_delete_stock_updt` */$$

/*!50003 CREATE */ /*!50017 DEFINER = 'ncsoftwa_re'@'%' */ /*!50003 TRIGGER `stockMov_delete_stock_updt` BEFORE DELETE ON `StockMovDetalle` FOR EACH ROW BEGIN
    DECLARE cantidad INT(11);
    SET cantidad = OLD.CantidadMSTKD;
	IF OLD.OrigenMSTKD = 1 THEN
		UPDATE Stock SET CantidadSTK = CantidadSTK - cantidad
		WHERE IdArticuloSTK = OLD.IdArticuloMSTKD AND IdLocalSTK = OLD.DestinoMSTKD;
	ELSEIF OLD.DestinoMSTKD = 2 THEN
		UPDATE Stock SET CantidadSTK = CantidadSTK + cantidad
		WHERE IdArticuloSTK = OLD.IdArticuloMSTKD AND IdLocalSTK = OLD.OrigenMSTKD;
	ELSE
		UPDATE Stock SET CantidadSTK = CantidadSTK - cantidad
		WHERE IdArticuloSTK = OLD.IdArticuloMSTKD AND IdLocalSTK = OLD.DestinoMSTKD;
		UPDATE Stock SET CantidadSTK = CantidadSTK + cantidad
		WHERE IdArticuloSTK = OLD.IdArticuloMSTKD AND IdLocalSTK = OLD.OrigenMSTKD;					
	END IF;
    END */$$


DELIMITER ;

/* Procedure structure for procedure `ArticulosItems_Actualizar` */

/*!50003 DROP PROCEDURE IF EXISTS  `ArticulosItems_Actualizar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `ArticulosItems_Actualizar`(
in p_id int(11),
IN p_descripcion varchar(50),
IN p_descripcionWeb VARCHAR(50),
IN p_activoWeb INT(1)
)
BEGIN
update ArticulosItems set DescripcionITE = p_descripcion, DescripcionWebITE = p_descripcionWeb,
ActivoWebITE = p_activoWeb
where IdItemITE = p_id 
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `ArticulosItems_Borrar` */

/*!50003 DROP PROCEDURE IF EXISTS  `ArticulosItems_Borrar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `ArticulosItems_Borrar`(IN p_id INT)
BEGIN
DELETE
FROM ArticulosItems
WHERE IdItemITE= p_id
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `ArticulosItems_Insertar` */

/*!50003 DROP PROCEDURE IF EXISTS  `ArticulosItems_Insertar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `ArticulosItems_Insertar`(
in p_id int,
IN p_descripcion VARCHAR(50),
IN p_descripcionWeb VARCHAR(50),
IN p_activoWeb INT(1)
)
BEGIN
INSERT INTO ArticulosItems(IdItemITE, DescripcionITE, DescripcionWebITE, ActivoWebITE)
VALUES(p_id, p_descripcion, p_descripcionWeb, p_activoWeb)
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `ArticulosItems_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `ArticulosItems_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `ArticulosItems_Listar`()
BEGIN
select *
FROM ArticulosItems
order by DescripcionITE
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Articulos_Actualizar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Articulos_Actualizar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Articulos_Actualizar`(
IN p_id VARCHAR(50),
IN p_idItem INT(11),
IN p_idColor INT(11),
IN p_talle VARCHAR(50),
IN p_idProveedor INT(11),
IN p_descripcion VARCHAR(50),
IN p_descripcionWeb VARCHAR(50),
IN p_precioCosto DECIMAL(19),
IN p_precioPublico DECIMAL(19),
IN p_precioMayor DECIMAL(19),
IN p_fecha DATETIME,
IN p_imagen VARCHAR(20),
IN p_imagenBack VARCHAR(20),
IN p_imagenColor VARCHAR(20),
IN p_activoWeb INT(1),
IN p_nuevo INT(1)
)
BEGIN
START TRANSACTION;
UPDATE Articulos SET IdItemART = p_idItem, IdColorART = p_idColor, TalleART = p_talle ,IdProveedorART = p_idProveedor, 
DescripcionART = p_descripcion, DescripcionWebART = p_descripcionWeb, PrecioCostoART = p_precioCosto, 
PrecioPublicoART = p_precioPublico, PrecioMayorART = p_precioMayor, FechaART = p_fecha, ImagenART = p_imagen, 
ImagenBackART = p_imagenBack, ImagenColorART = p_imagenColor, ActivoWebART = p_activoWeb, NuevoART = p_nuevo
WHERE IdArticuloART = p_id;
COMMIT;
IF p_imagen IS NOT NULL THEN
	IF p_imagenBack ='' AND p_imagenColor ='' THEN
		UPDATE Articulos SET ImagenART = p_imagen, ImagenBackART = p_imagenBack, ImagenColorART = p_imagenColor  
		WHERE IdArticuloART LIKE CONCAT(LEFT(p_id,5),'%');	
	ELSEIF p_imagenBack <>'' AND p_imagenColor ='' THEN
		UPDATE Articulos SET ImagenART = p_imagen, ImagenBackART = p_imagenBack, ImagenColorART = p_imagenColor
		WHERE IdArticuloART LIKE CONCAT(LEFT(p_id,5),'%');	
	ELSEIF p_imagenBack ='' AND p_imagenColor <>'' THEN
		UPDATE Articulos SET ImagenART = p_imagen, ImagenBackART = p_imagenBack, ImagenColorART = p_imagenColor
		WHERE IdArticuloART LIKE CONCAT(LEFT(p_id,7),'%');	
	ELSEIF p_imagenBack <>'' AND p_imagenColor <>'' THEN
		UPDATE Articulos SET ImagenART = p_imagen, ImagenBackART = p_imagenBack, ImagenColorART = p_imagenColor 
		WHERE IdArticuloART LIKE CONCAT(LEFT(p_id,7),'%');	
	END IF;	
END IF;	
END */$$
DELIMITER ;

/* Procedure structure for procedure `Articulos_Actualizar_Precio` */

/*!50003 DROP PROCEDURE IF EXISTS  `Articulos_Actualizar_Precio` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Articulos_Actualizar_Precio`(
in p_id varchar(50),
IN p_precioPublico DECIMAL(19)
)
BEGIN
update Articulos set PrecioPublicoART = p_precioPublico
where IdArticuloART like concat(p_id, '%')
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Articulos_Borrar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Articulos_Borrar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Articulos_Borrar`(IN p_id varchar(50))
BEGIN
DELETE
FROM Articulos
WHERE IdArticuloART = p_id
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Articulos_Borrar_stock_0` */

/*!50003 DROP PROCEDURE IF EXISTS  `Articulos_Borrar_stock_0` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Articulos_Borrar_stock_0`()
BEGIN
  DECLARE done INT DEFAULT 0;
  DECLARE codigo_articulo VARCHAR(16);
  DECLARE cur1 CURSOR FOR SELECT Codigo FROM ArticulosBorrarCons WHERE Cantidad = 0;
  DECLARE CONTINUE HANDLER FOR SQLSTATE '02000' SET done = 1;
  OPEN cur1;
  REPEAT
    FETCH cur1 INTO codigo_articulo;
    IF NOT done THEN
	DELETE FROM Articulos WHERE IdArticuloART LIKE CONCAT(codigo_articulo,'%');
    END IF;
  UNTIL done END REPEAT;
  CLOSE cur1;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Articulos_Insertar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Articulos_Insertar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Articulos_Insertar`(
IN p_id VARCHAR(50),
IN p_idItem INT(11),
IN p_idColor INT(11),
IN p_talle VARCHAR(50),
IN p_idProveedor INT(11),
IN p_descripcion VARCHAR(55),
IN p_descripcionWeb VARCHAR(50),
IN p_precioCosto DECIMAL(19),
IN p_precioPublico DECIMAL(19),
IN p_precioMayor DECIMAL(19),
IN p_fecha DATETIME,
IN p_imagen VARCHAR(20),
IN p_imagenBack VARCHAR(20),
IN p_imagenColor VARCHAR(20),
IN p_activoWeb INT(1),
IN p_nuevo INT(1)
)
BEGIN
INSERT INTO Articulos(IdArticuloART, IdItemART, IdColorART, TalleART, IdProveedorART, DescripcionART,
DescripcionWebART, PrecioCostoART, PrecioPublicoART, PrecioMayorART, FechaART, 
ImagenART, ImagenBackART, ImagenColorART, ActivoWebART, NuevoART)
VALUES(p_id, p_idItem, p_idColor, p_talle, p_idProveedor, p_descripcion, p_descripcionWeb,
p_precioCosto, p_precioPublico, p_precioMayor, p_fecha, p_imagen, p_imagenBack, p_imagenColor, p_activoWeb, p_nuevo)
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Articulos_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Articulos_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Articulos_Listar`()
BEGIN
select *
FROM Articulos
order by DescripcionART
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Articulos_Listar_Nuevos` */

/*!50003 DROP PROCEDURE IF EXISTS  `Articulos_Listar_Nuevos` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Articulos_Listar_Nuevos`()
BEGIN
select `FechaART`, `IdArticuloART`, `DescripcionART`, `NuevoART`
FROM Articulos
order by FechaART asc
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Clientes_Actualizar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Clientes_Actualizar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Clientes_Actualizar`(
in p_id int(11),
IN p_razon varchar(50),
IN p_cuit VARCHAR(50),
IN p_direccion VARCHAR(50),
IN p_localidad VARCHAR(50),
IN p_provincia VARCHAR(50),
IN p_transporte VARCHAR(50),
IN p_contacto VARCHAR(50),
IN p_telefono VARCHAR(50),
IN p_movil VARCHAR(50),
IN p_correo VARCHAR(50),
IN p_fecha VARCHAR(50)
)
BEGIN
update Clientes set RazonSocialCLI = p_razon, CUIT = p_cuit, DireccionCLI = p_direccion, 
LocalidadCLI = p_localidad, ProvinciaCLI = p_provincia, TransporteCLI = p_transporte,
ContactoCLI = p_contacto, TelefonoCLI = p_telefono, MovilCLI = p_movil, CorreoCLI = p_correo,
FechaNacCLI = p_fecha
where IdClienteCLI = p_id 
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Clientes_Borrar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Clientes_Borrar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Clientes_Borrar`(IN p_id INT)
BEGIN
DELETE
FROM Clientes
WHERE IdClienteCLI = p_id
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Clientes_Insertar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Clientes_Insertar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Clientes_Insertar`(
IN p_id INT(11),
IN p_razon VARCHAR(50),
IN p_cuit VARCHAR(50),
IN p_direccion VARCHAR(50),
IN p_localidad VARCHAR(50),
IN p_provincia VARCHAR(50),
IN p_transporte VARCHAR(50),
IN p_contacto VARCHAR(50),
IN p_telefono VARCHAR(50),
IN p_movil VARCHAR(50),
IN p_correo VARCHAR(50),
IN p_fecha VARCHAR(50)
)
BEGIN
INSERT INTO Clientes(IdClienteCLI, RazonSocialCLI, CUIT, DireccionCLI, LocalidadCLI, ProvinciaCLI,
TransporteCLI, ContactoCLI, TelefonoCLI, MovilCLI, CorreoCLI, FechaNacCLI)
VALUES(p_id, p_razon, p_cuit, p_direccion, p_localidad, p_provincia, p_transporte, p_contacto,
p_telefono, p_movil, p_correo, p_fecha)
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Clientes_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Clientes_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Clientes_Listar`()
BEGIN
select *
FROM Clientes
order by RazonSocialCLI
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Colores_Actualizar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Colores_Actualizar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Colores_Actualizar`(
IN p_id INT(11),
IN p_descripcion VARCHAR(50),
IN p_hex VARCHAR(50)
)
BEGIN
UPDATE Colores SET DescripcionCOL = p_descripcion, HexCOL = p_hex
WHERE IdColorCOL = p_id 
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Colores_Borrar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Colores_Borrar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Colores_Borrar`(IN p_id INT)
BEGIN
DELETE
FROM Colores
WHERE IdColorCOL= p_id
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Colores_Insertar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Colores_Insertar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Colores_Insertar`(
IN p_id INT,
IN p_descripcion VARCHAR(50),
IN p_hex VARCHAR(50)
)
BEGIN
INSERT INTO Colores(IdColorCOL, DescripcionCOL, HexCOL)
VALUES(p_id, p_descripcion, p_hex)
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Colores_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Colores_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Colores_Listar`()
BEGIN
select *
FROM Colores
order by DescripcionCOL
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `DatosPos_Borrar` */

/*!50003 DROP PROCEDURE IF EXISTS  `DatosPos_Borrar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `DatosPos_Borrar`()
BEGIN
DELETE FROM Articulos;
DELETE FROM Clientes;
DELETE FROM FormasPago;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Fallidas_Borrar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Fallidas_Borrar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Fallidas_Borrar`(
IN p_descripcion varchar(20),
in p_accion varchar(20)
)
BEGIN
DELETE
FROM Fallidas
where DescripcionFLL = p_descripcion and AccionFLL = p_accion
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Fallidas_GetByPK` */

/*!50003 DROP PROCEDURE IF EXISTS  `Fallidas_GetByPK` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Fallidas_GetByPK`(IN p_id INT)
BEGIN
select *
FROM Fallidas
WHERE `Id` = p_id
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Fallidas_Insertar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Fallidas_Insertar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Fallidas_Insertar`(
IN p_id int(10),
IN p_descripcion VARCHAR(20),
in p_accion varchar(20)
)
BEGIN
INSERT INTO Fallidas(Id, DescripcionFLL, AccionFLL)
VALUES(p_id, p_descripcion, p_accion)
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Fallidas_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Fallidas_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Fallidas_Listar`(
in p_descripcion varchar(20),
IN p_accion VARCHAR(20)
)
BEGIN
select * from Fallidas
where DescripcionFLL = p_descripcion and AccionFLL = p_accion
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `FondoCaja_Actualizar` */

/*!50003 DROP PROCEDURE IF EXISTS  `FondoCaja_Actualizar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `FondoCaja_Actualizar`(
in p_fecha VARCHAR(50),
IN p_pc int(11),
in p_importe double
)
BEGIN
update FondoCaja set `FechaFONP` = p_fecha, `IdPcFONP` = p_pc, `ImporteFONP` = p_importe
where `FechaFONP` = p_fecha AND `IdPcFONP` = p_pc
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `FondoCaja_Borrar` */

/*!50003 DROP PROCEDURE IF EXISTS  `FondoCaja_Borrar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `FondoCaja_Borrar`(
IN p_fecha varchar(50),
IN p_pc INT(11)
)
BEGIN
DELETE
FROM FondoCaja
WHERE `FechaFONP` = p_fecha and `IdPcFONP` = p_pc
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `FondoCaja_Inicial` */

/*!50003 DROP PROCEDURE IF EXISTS  `FondoCaja_Inicial` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `FondoCaja_Inicial`(
in p_fecha varchar(50),
in p_idPc int(11)
)
BEGIN
SELECT `ImporteFONP` FROM FondoCaja
WHERE `FechaFONP` = (SELECT MAX(`FechaFONP`) FROM FondoCaja WHERE `FechaFONP` < p_fecha)
AND `IdPcFONP` = p_idPc
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `FondoCaja_Inicial_Final` */

/*!50003 DROP PROCEDURE IF EXISTS  `FondoCaja_Inicial_Final` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `FondoCaja_Inicial_Final`(
in p_fecha varchar(50),
in p_idPc int(11)
)
BEGIN
SELECT `ImporteFONP` FROM FondoCaja
WHERE `FechaFONP` = (SELECT MAX(`FechaFONP`) FROM FondoCaja WHERE `FechaFONP` < p_fecha)
AND `IdPcFONP` = p_idPc
;
SELECT `FechaFONP`, `IdPcFONP`, `ImporteFONP` FROM FondoCaja
WHERE `FechaFONP` = p_fecha
AND `IdPcFONP` = p_idPc
;
SELECT * FROM `TesoreriaMovimientos`
WHERE `FechaTESM` = p_fecha
AND `IdPcTESM` = p_idPc
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `FondoCaja_Insertar` */

/*!50003 DROP PROCEDURE IF EXISTS  `FondoCaja_Insertar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `FondoCaja_Insertar`(
IN p_fecha DATETIME,
IN p_pc INT(11),
IN p_importe DOUBLE
)
BEGIN
INSERT INTO FondoCaja(`FechaFONP`, `IdPcFONP`, `ImporteFONP`)
VALUES(p_fecha, p_pc, p_importe)
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `FondoCaja_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `FondoCaja_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `FondoCaja_Listar`()
BEGIN
SELECT *
FROM FondoCaja
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `FondoCaja_ListarCons` */

/*!50003 DROP PROCEDURE IF EXISTS  `FondoCaja_ListarCons` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `FondoCaja_ListarCons`()
BEGIN
SELECT *
FROM FondoCajaCons
order by `FechaFONP` desc
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `FondoCaja_PK` */

/*!50003 DROP PROCEDURE IF EXISTS  `FondoCaja_PK` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `FondoCaja_PK`(
IN p_fecha DATETIME,
IN p_pc INT(11)
)
BEGIN
SELECT *
FROM FondoCaja
WHERE `FechaFONP` = p_fecha and `IdPcFONP` = p_pc
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `FormasPago_Actualizar` */

/*!50003 DROP PROCEDURE IF EXISTS  `FormasPago_Actualizar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `FormasPago_Actualizar`(
in p_id int(11),
IN p_descripcion varchar(50))
BEGIN
update FormasPago set DescripcionFOR = p_descripcion
where IdFormaPagoFOR = p_id 
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `FormasPago_Borrar` */

/*!50003 DROP PROCEDURE IF EXISTS  `FormasPago_Borrar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `FormasPago_Borrar`(IN p_id INT)
BEGIN
DELETE
FROM FormasPago
WHERE IdFormaPagoFOR= p_id
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `FormasPago_Insertar` */

/*!50003 DROP PROCEDURE IF EXISTS  `FormasPago_Insertar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `FormasPago_Insertar`(
in p_id int(11),
IN p_descripcion VARCHAR(50)
)
BEGIN
INSERT INTO FormasPago(IdFormaPagoFOR, DescripcionFOR)
VALUES(p_id, p_descripcion)
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `FormasPago_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `FormasPago_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `FormasPago_Listar`()
BEGIN
select *
FROM FormasPago
order by DescripcionFOR
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Locales_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Locales_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Locales_Listar`()
BEGIN
select *
FROM Locales
order by IdLocalLOC
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Pcs_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Pcs_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Pcs_Listar`()
BEGIN
select *
FROM Pc
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `PedidoCons` */

/*!50003 DROP PROCEDURE IF EXISTS  `PedidoCons` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `PedidoCons`(
in p_fechaDesde varchar(50)
)
BEGIN
drop table if exists PedidoVentas;
CREATE TABLE PedidoVentas
AS SELECT
	`VentasDetalle`.`IdArticuloDVEN`
    , SUM(`VentasDetalle`.`CantidadDVEN`) as Venta
FROM
    `ncsoftwa_re`.`VentasDetalle`
    INNER JOIN `ncsoftwa_re`.`Ventas` 
        ON (`VentasDetalle`.`IdVentaDVEN` = `Ventas`.`IdVentaVEN`)
WHERE FechaVEN >=p_fechaDesde         
GROUP BY `VentasDetalle`.`IdArticuloDVEN`;
SELECT
	`PedidoStockCons`.`RazonSocialPRO` as Proveedor
	, `PedidoStockCons`.`IdArticuloSTK` as Articulo
	, `PedidoStockCons`.`DescripcionART` AS Descripcion
	, `PedidoVentas`.`Venta`
	, `PedidoStockCons`.`Stock`
	, `PedidoStockCons`.`Costo`
	, `PedidoStockCons`.`Publico`
FROM
    `ncsoftwa_re`.`PedidoVentas`
    RIGHT JOIN `ncsoftwa_re`.`PedidoStockCons` 
        ON (`PedidoVentas`.`IdArticuloDVEN` = `PedidoStockCons`.`IdArticuloSTK`)
    order by `PedidoStockCons`.`DescripcionART`    
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `PedidoCons2` */

/*!50003 DROP PROCEDURE IF EXISTS  `PedidoCons2` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `PedidoCons2`(
in p_fechaDesde varchar(50)
)
BEGIN
drop table if exists PedidoVentas;
CREATE TABLE PedidoVentas
AS SELECT
	`VentasDetalle`.`IdArticuloDVEN`
    , SUM(`VentasDetalle`.`CantidadDVEN`) as Venta
FROM
    `ncsoftwa_karminna`.`VentasDetalle`
    INNER JOIN `ncsoftwa_karminna`.`Ventas` 
        ON (`VentasDetalle`.`IdVentaDVEN` = `Ventas`.`IdVentaVEN`)
WHERE FechaVEN >=p_fechaDesde         
GROUP BY `VentasDetalle`.`IdArticuloDVEN`;
SELECT
	`PedidoStockCons`.`RazonSocialPRO` as Proveedor
	, `PedidoStockCons`.`IdArticuloSTK` as Articulo
	, `PedidoVentas`.`Venta`
	, `PedidoStockCons`.`Stock`
	, `PedidoStockCons`.`Costo`
	, `PedidoStockCons`.`Publico`
FROM
    `ncsoftwa_karminna`.`PedidoVentas`
    RIGHT JOIN `ncsoftwa_karminna`.`PedidoStockCons` 
        ON (`PedidoVentas`.`IdArticuloDVEN` = `PedidoStockCons`.`IdArticuloSTK`);
END */$$
DELIMITER ;

/* Procedure structure for procedure `Probando` */

/*!50003 DROP PROCEDURE IF EXISTS  `Probando` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Probando`(
in p_locales varchar(200),
in p_forma int(2),
in p_fechaDesde varchar(50),
IN p_fechaHasta VARCHAR(50)
)
BEGIN
  DECLARE locales VARCHAR(50);
  DECLARE forma INT(2);
  DECLARE fechaDesde VARCHAR(50);
  DECLARE fechaHasta VARCHAR(50);
  SET locales = p_locales;
  SET forma = p_forma;
  SET fechaDesde = p_fechaDesde;
  SET fechaHasta = p_fechaHasta;
  
  SET @QUERY = CONCAT("SELECT 
	NombreLOC, SUM(Venta) AS Venta, SUM(Costo) AS Costo, SUM(Prendas) AS Prendas, 
	(SUM(Venta) - SUM(Costo)) AS 'Utilidad bruta', ((SUM(Venta)/SUM(Costo))-1) AS 'Valor agregado'
	FROM VentasPesosCons 
	WHERE (",locales,") AND (FechaVEN >='",fechaDesde,"' AND FechaVEN <='",fechaHasta,"')
	GROUP BY NombreLOC");
  
  PREPARE smpt FROM @QUERY;
  EXECUTE smpt;
  
END */$$
DELIMITER ;

/* Procedure structure for procedure `Probando_Ejecutar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Probando_Ejecutar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Probando_Ejecutar`()
BEGIN
  declare locales varchar(50);
  DECLARE forma int(2);
  DECLARE fechaDesde varchar(50);
  DECLARE fechaHasta VARCHAR(50);
  SET locales = 'IdLocalLOC =14 OR IdLocalLOC =13' ;
  SET forma = 1 ;
  SET fechaDesde = '2013-12-26 00:00:00' ;
  SET fechaHasta = '2013-12-29 00:00:00' ;
  
call Probando(locales, forma, fechaDesde);
  
END */$$
DELIMITER ;

/* Procedure structure for procedure `Proveedores_Actualizar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Proveedores_Actualizar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Proveedores_Actualizar`(
in p_id int(11),
IN p_razon varchar(50),
IN p_direccion VARCHAR(50),
IN p_codigo VARCHAR(50),
IN p_telefono VARCHAR(50),
IN p_contacto VARCHAR(50)
)
BEGIN
update Proveedores set RazonSocialPRO = p_razon, DireccionPRO = p_direccion, CodigoPostalPRO = p_codigo,
TelefonoPRO = p_telefono, ContactoPRO = p_contacto
where IdProveedorPRO = p_id 
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Proveedores_Borrar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Proveedores_Borrar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Proveedores_Borrar`(IN p_id INT)
BEGIN
DELETE
FROM Proveedores
WHERE IdProveedorPRO = p_id
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Proveedores_Insertar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Proveedores_Insertar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Proveedores_Insertar`(
IN p_id int,
IN p_razon VARCHAR(50),
IN p_direccion VARCHAR(50),
IN p_codigo VARCHAR(50),
IN p_telefono VARCHAR(50),
IN p_contacto VARCHAR(50)
)
BEGIN
INSERT INTO Proveedores(IdProveedorPRO, RazonSocialPRO, DireccionPRO , CodigoPostalPRO, TelefonoPRO, ContactoPRO)
VALUES(p_id, p_razon, p_direccion, p_codigo, p_telefono, p_contacto)
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Proveedores_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Proveedores_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Proveedores_Listar`()
BEGIN
select *
FROM Proveedores
order by RazonSocialPRO
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `StockMovDetalle_Actualizar` */

/*!50003 DROP PROCEDURE IF EXISTS  `StockMovDetalle_Actualizar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `StockMovDetalle_Actualizar`(
in p_id_detalle int,
in p_articulo varchar(50),
in p_cantidad int,
in p_compensa tinyint
)
BEGIN
UPDATE StockMovDetalle SET IdArticuloMSTKD = p_articulo, CantidadMSTKD = p_cantidad, CompensaMSTKD = p_compensa
WHERE IdMSTKD = p_id_detalle
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `StockMovDetalle_Borrar` */

/*!50003 DROP PROCEDURE IF EXISTS  `StockMovDetalle_Borrar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `StockMovDetalle_Borrar`(IN p_id_detalle INT)
BEGIN
DELETE
FROM StockMovDetalle
WHERE IdMSTKD= p_id_detalle
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `StockMovDetalle_Insertar` */

/*!50003 DROP PROCEDURE IF EXISTS  `StockMovDetalle_Insertar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `StockMovDetalle_Insertar`(
IN p_id_detalle INT,
IN p_id_mov INT,
IN p_articulo VARCHAR(50),
IN p_cantidad INT,
IN p_compensa TINYINT,
IN p_origen TINYINT,
IN p_destino TINYINT
)
BEGIN
INSERT INTO StockMovDetalle(IdMSTKD, IdMovMSTKD, IdArticuloMSTKD, CantidadMSTKD, CompensaMSTKD,
OrigenMSTKD, DestinoMSTKD)
VALUES(p_id_detalle, p_id_mov, p_articulo, p_cantidad, p_compensa, p_origen, p_destino)
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `StockMov_Actualizar` */

/*!50003 DROP PROCEDURE IF EXISTS  `StockMov_Actualizar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `StockMov_Actualizar`(
in p_id int,
IN p_fecha datetime,
in p_origen int,
in p_destino int,
IN p_compensa TINYINT
)
BEGIN
update StockMov set FechaMSTK = p_fecha, OrigenMSTK = p_origen, DestinoMSTK = p_destino, CompensaMSTK = p_compensa
where IdMovMSTK = p_id
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `StockMov_Borrar` */

/*!50003 DROP PROCEDURE IF EXISTS  `StockMov_Borrar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `StockMov_Borrar`(IN p_id INT)
BEGIN
DELETE
FROM StockMov
WHERE IdMovMSTK= p_id
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `StockMov_Cons` */

/*!50003 DROP PROCEDURE IF EXISTS  `StockMov_Cons` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `StockMov_Cons`(
in p_fecha_desde varchar(50),
IN p_fecha_hasta VARCHAR(50),
in p_id_local int(3),
in p_tipo_mov varchar(15),
in p_movimiento varchar(8)
)
BEGIN
IF p_tipo_mov ='movimientos' THEN
	IF p_movimiento ='entradas' THEN
		select * from StockMov
		WHERE (`FechaMSTK` >= p_fecha_desde and `FechaMSTK` <= p_fecha_hasta) 
		and `DestinoMSTK` = p_id_local and `CompensaMSTK` = 0
		order by FechaMSTK;
		
		select * from StockMovCons
		WHERE (`FechaMSTK` >= p_fecha_desde AND `FechaMSTK` <= p_fecha_hasta) 
		AND `DestinoMSTK` = p_id_local AND `CompensaMSTK` = 0
		order by DescripcionART;
	elseif p_movimiento ='salidas' THEN
		SELECT * FROM StockMov
		WHERE (`FechaMSTK` >= p_fecha_desde AND `FechaMSTK` <= p_fecha_hasta) 
		AND `OrigenMSTK` = p_id_local AND `CompensaMSTK` = 0
		ORDER BY FechaMSTK;
		
		SELECT * FROM StockMovCons
		WHERE (`FechaMSTK` >= p_fecha_desde AND `FechaMSTK` <= p_fecha_hasta) 
		AND `OrigenMSTK` = p_id_local AND `CompensaMSTK` = 0
		ORDER BY DescripcionART;
	else
		SELECT * FROM StockMov
		WHERE (`FechaMSTK` >= p_fecha_desde AND `FechaMSTK` <= p_fecha_hasta) 
		AND (`OrigenMSTK` = p_id_local OR `DestinoMSTK` = p_id_local) AND `CompensaMSTK` = 0
		ORDER BY FechaMSTK;
		
		SELECT * FROM StockMovCons
		WHERE (`FechaMSTK` >= p_fecha_desde AND `FechaMSTK` <= p_fecha_hasta) 
		AND (`OrigenMSTK` = p_id_local OR `DestinoMSTK` = p_id_local) AND `CompensaMSTK` = 0
		ORDER BY DescripcionART;	
	end if;
else
	SELECT * FROM StockMov
	WHERE (`FechaMSTK` >= p_fecha_desde AND `FechaMSTK` <= p_fecha_hasta) 
	AND `DestinoMSTK` = p_id_local AND `CompensaMSTK` = 1
	ORDER BY FechaMSTK;
	
	SELECT * FROM StockMovCons
	WHERE (`FechaMSTK` >= p_fecha_desde AND `FechaMSTK` <= p_fecha_hasta) 
	AND `DestinoMSTK` = p_id_local AND `CompensaMSTK` = 1
	ORDER BY DescripcionART;
end if;
END */$$
DELIMITER ;

/* Procedure structure for procedure `StockMov_Insertar` */

/*!50003 DROP PROCEDURE IF EXISTS  `StockMov_Insertar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `StockMov_Insertar`(
in p_id int,
IN p_fecha datetime,
in p_origen int,
in p_destino int,
IN p_compensa tinyint
)
BEGIN
INSERT INTO StockMov(IdMovMSTK, FechaMSTK, OrigenMSTK, DestinoMSTK, CompensaMSTK)
VALUES(p_id, p_fecha, p_origen, p_destino, p_compensa)
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `StockMov_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `StockMov_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `StockMov_Listar`(
)
BEGIN
select * from StockMov;
select * from StockMovDetalle;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Stock_Actualizar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Stock_Actualizar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Stock_Actualizar`(
in p_id_articulo varchar(50),
IN p_id_local int,
in p_cantidad int
)
BEGIN
update Stock set CantidadSTK = p_cantidad
where IdArticuloSTK = p_id_articulo and IdLocalSTK = p_id_local
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Stock_Cons` */

/*!50003 DROP PROCEDURE IF EXISTS  `Stock_Cons` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Stock_Cons`(
IN p_locales VARCHAR(200),
IN p_proveedor INT(3),
IN p_articulo VARCHAR(9),
IN p_descripcion VARCHAR(200),
in p_activoWeb int(1)
)
BEGIN
	IF p_proveedor<>0 THEN
		if p_activoWeb = 0 then
			SET @QUERY = CONCAT("SELECT Articulo, Descripcion, Cantidad, Proveedor, NombreLOC FROM StockCons 
			WHERE ((",p_locales,") AND IdProveedorPRO ='",p_proveedor,"') AND (Articulo LIKE '",p_articulo,"%'
				AND Descripcion LIKE '%",p_descripcion,"%')
			ORDER BY Descripcion");	
		else
			SET @QUERY = CONCAT("SELECT Articulo, Descripcion, Cantidad, Proveedor, NombreLOC FROM StockCons 
			WHERE ((",p_locales,") AND IdProveedorPRO ='",p_proveedor,"') AND (Articulo LIKE '",p_articulo,"%'
				AND Descripcion LIKE '%",p_descripcion,"%' AND ActivoWebART = 1 AND ImagenART<>'')
			ORDER BY Descripcion");			
		end if;
	ELSE
		IF p_activoWeb = 0 THEN
			SET @QUERY = CONCAT("SELECT Articulo, Descripcion, Cantidad, Proveedor, NombreLOC FROM StockCons 
			WHERE ((",p_locales,") AND (Articulo LIKE '",p_articulo,"%'
				AND Descripcion LIKE '%",p_descripcion,"%'))
			ORDER BY Descripcion");
		ELSE
			SET @QUERY = CONCAT("SELECT Articulo, Descripcion, Cantidad, Proveedor, NombreLOC FROM StockCons 
			WHERE ((",p_locales,") AND (Articulo LIKE '",p_articulo,"%'
				AND Descripcion LIKE '%",p_descripcion,"%' AND ActivoWebART = 1 AND ImagenART<>''))
			ORDER BY Descripcion");		
		END IF;
	END IF;
  
  PREPARE smpt FROM @QUERY;
  EXECUTE smpt;
  
END */$$
DELIMITER ;

/* Procedure structure for procedure `Stock_Insertar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Stock_Insertar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Stock_Insertar`(
in p_id_articulo varchar(50),
in p_id_local int,
IN p_cantidad int
)
BEGIN
INSERT INTO Stock(IdArticuloSTK, IdLocalSTK, CantidadSTK)
VALUES(p_id_articulo, p_id_local, p_cantidad)
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Stock_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Stock_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Stock_Listar`(
)
BEGIN
select *
FROM Stock
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Stock_WhereLocal` */

/*!50003 DROP PROCEDURE IF EXISTS  `Stock_WhereLocal` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Stock_WhereLocal`(
in p_locales varchar(100)
)
BEGIN
select *
FROM Stock
p_locales
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `TesoreriaMovFechaLocal_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `TesoreriaMovFechaLocal_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `TesoreriaMovFechaLocal_Listar`(
in p_fecha datetime,
in p_pc int(11)
)
BEGIN
select *
FROM TesoreriaMovimientos
where `FechaTESM` = p_fecha and `IdPcTESM` = p_pc
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `TesoreriaMov_Actualizar` */

/*!50003 DROP PROCEDURE IF EXISTS  `TesoreriaMov_Actualizar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `TesoreriaMov_Actualizar`(
in p_id int(11),
in p_fecha datetime,
IN p_pc int(11),
IN p_detalle varchar(50),
in p_importe double
)
BEGIN
update TesoreriaMovimientos set `FechaTESM` = p_fecha, `IdPcTESM` = p_pc, 
`DetalleTESM` = p_detalle, `ImporteTESM` = p_importe
where `IdMovTESM` = p_id 
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `TesoreriaMov_Borrar` */

/*!50003 DROP PROCEDURE IF EXISTS  `TesoreriaMov_Borrar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `TesoreriaMov_Borrar`(IN p_id INT)
BEGIN
DELETE
FROM TesoreriaMovimientos
WHERE `IdMovTESM` = p_id
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `TesoreriaMov_Insertar` */

/*!50003 DROP PROCEDURE IF EXISTS  `TesoreriaMov_Insertar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `TesoreriaMov_Insertar`(
IN p_id INT(11),
IN p_fecha DATETIME,
IN p_pc INT(11),
IN p_detalle VARCHAR(50),
IN p_importe DOUBLE
)
BEGIN
INSERT INTO TesoreriaMovimientos(`IdMovTESM`, `FechaTESM`, `IdPcTESM`, `DetalleTESM`, `ImporteTESM`)
VALUES(p_id, p_fecha, p_pc, p_detalle, p_importe)
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `TesoreriaMov_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `TesoreriaMov_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `TesoreriaMov_Listar`()
BEGIN
select *
FROM TesoreriaMovimientos
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasDetalleCons_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasDetalleCons_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasDetalleCons_Listar`(
in p_locales varchar(200),
in p_forma int(2),
in p_fechaDesde varchar(50),
IN p_fechaHasta VARCHAR(50),
in p_opcion int(1),
in p_parametros varchar(200)
)
BEGIN
  DECLARE locales VARCHAR(50);
  DECLARE forma INT(2);
  DECLARE fechaDesde VARCHAR(50);
  DECLARE fechaHasta VARCHAR(50);
  DECLARE opcion int(1);
  DECLARE parametros VARCHAR(200);
  SET locales = p_locales;
  SET forma = p_forma;
  SET fechaDesde = p_fechaDesde;
  SET fechaHasta = p_fechaHasta;
  set opcion = p_opcion;
  set parametros = p_parametros;
  
if forma =99 then	/* todas las formas de pago */
	if opcion = 1 then 	/* busco por IdArticuloDVEN */
		SET @QUERY = CONCAT("SELECT 
		NombreLOC AS Local, FechaVEN AS Fecha, IdArticuloDVEN AS Artículo, DescripcionART AS Descripción, 
		CantidadDVEN AS Cantidad, PrecioPublicoDVEN AS Precio, DescripcionFOR AS 'Forma de pago'
		FROM VentasDetalleCons 
		WHERE (",locales,") AND (FechaVEN >='",fechaDesde,"' AND FechaVEN <='",fechaHasta,"') 
		AND IdArticuloDVEN LIKE '",parametros,"%'");
	else
		/* busco por IdArticuloDVEN */
		SET @QUERY = CONCAT("SELECT 
		NombreLOC AS Local, FechaVEN AS Fecha, IdArticuloDVEN AS Artículo, DescripcionART AS Descripción, 
		CantidadDVEN AS Cantidad, PrecioPublicoDVEN AS Precio, DescripcionFOR AS 'Forma de pago'
		FROM VentasDetalleCons 
		WHERE (",locales,") AND (FechaVEN >='",fechaDesde,"' AND FechaVEN <='",fechaHasta,"') 
		AND DescripcionART LIKE '%",parametros,"%'");
	end if;
else
	IF opcion = 1 THEN 	/* busco por IdArticuloDVEN */
		SET @QUERY = CONCAT("SELECT 
		NombreLOC AS Local, FechaVEN AS Fecha, IdArticuloDVEN AS Artículo, DescripcionART AS Descripción, 
		CantidadDVEN AS Cantidad, PrecioPublicoDVEN AS Precio, DescripcionFOR AS 'Forma de pago'
		FROM VentasDetalleCons 
		WHERE (",locales,") AND (FechaVEN >='",fechaDesde,"' AND FechaVEN <='",fechaHasta,"') 
		AND IdArticuloDVEN LIKE '",parametros,"%' AND IdFormaPagoFOR =",p_forma);
	ELSE
		/* busco por IdArticuloDVEN */
		SET @QUERY = CONCAT("SELECT 
		NombreLOC AS Local, FechaVEN AS Fecha, IdArticuloDVEN AS Artículo, DescripcionART AS Descripción, 
		CantidadDVEN AS Cantidad, PrecioPublicoDVEN AS Precio, DescripcionFOR AS 'Forma de pago'
		FROM VentasDetalleCons 
		WHERE (",locales,") AND (FechaVEN >='",fechaDesde,"' AND FechaVEN <='",fechaHasta,"') 
		AND DescripcionART LIKE '%",parametros,"%' AND IdFormaPagoFOR =",p_forma);
	END IF;	
END IF;
  
  PREPARE smpt FROM @QUERY;
  EXECUTE smpt;
  
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasDetalleFallidas_Borrar` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasDetalleFallidas_Borrar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasDetalleFallidas_Borrar`(
in p_id int,
in p_accion varchar(20)
)
BEGIN
DELETE FROM VentasDetalleFallidas
where Id = p_id and Accion = p_accion
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasDetalleFallidas_BorrarByAccion` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasDetalleFallidas_BorrarByAccion` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasDetalleFallidas_BorrarByAccion`(
in p_accion varchar(20)
)
BEGIN
DELETE FROM VentasDetalleFallidas
where Accion = p_accion
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasDetalleFallidas_BorrarCopia` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasDetalleFallidas_BorrarCopia` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasDetalleFallidas_BorrarCopia`()
BEGIN
DELETE
FROM VentasDetalleFallidas
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasDetalleFallidas_DelByVenta` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasDetalleFallidas_DelByVenta` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasDetalleFallidas_DelByVenta`(
in p_id_venta int
)
BEGIN
DELETE FROM VentasDetalleFallidas
where IdVenta = p_id_venta
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasDetalleFallidas_Existe` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasDetalleFallidas_Existe` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasDetalleFallidas_Existe`(
IN p_id int,
in p_accion varchar(20)
)
BEGIN
select * from VentasDetalleFallidas
where Id = p_id and Accion = p_accion
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasDetalleFallidas_Get` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasDetalleFallidas_Get` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasDetalleFallidas_Get`(
in p_accion varchar(20)
)
BEGIN
select * from VentasDetalleFallidas
where Accion = p_accion
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasDetalleFallidas_GetByAccion` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasDetalleFallidas_GetByAccion` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasDetalleFallidas_GetByAccion`(
in p_accion varchar(20)
)
BEGIN
select * from VentasDetalleFallidas
where Accion = p_accion
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasDetalleFallidas_Insertar` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasDetalleFallidas_Insertar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasDetalleFallidas_Insertar`(
IN p_id int(10),
IN p_id_venta INT(10),
in p_accion varchar(20)
)
BEGIN
INSERT INTO VentasDetalleFallidas(Id, IdVenta, Accion)
VALUES(p_id, p_id_venta, p_accion)
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasDetalleFallidas_SelectByVenta` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasDetalleFallidas_SelectByVenta` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasDetalleFallidas_SelectByVenta`(
IN p_id_venta INT(10)
)
BEGIN
Select IdVenta from VentasDetalleFallidas
where IdVenta = p_id_venta
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasDetalle_Actualizar` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasDetalle_Actualizar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasDetalle_Actualizar`(
in p_id_detalle int,
in p_articulo varchar(50),
in p_cantidad int,
in p_publico double,
IN p_costo DOUBLE,
IN p_mayor DOUBLE,
in p_forma_pago int,
in p_nro_cupon int,
in p_nro_factura int,
in p_id_empleado int,
in p_liquidado bit,
in p_devolucion bit
)
BEGIN
UPDATE VentasDetalle SET `IdArticuloDVEN` = p_articulo, `CantidadDVEN` = p_cantidad, `PrecioPublicoDVEN` = p_publico,
`PrecioCostoDVEN` = p_costo, `PrecioMayorDVEN` = p_mayor, `IdFormaPagoDVEN` = p_forma_pago, 
`NroCuponDVEN` = p_nro_cupon, `NroFacturaDVEN` = p_nro_factura, `IdEmpleadoDVEN` = p_id_empleado,
`LiquidadoDVEN` = p_liquidado, `DevolucionDVEN` = p_devolucion
WHERE `IdDVEN` = p_id_detalle
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasDetalle_Borrar` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasDetalle_Borrar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasDetalle_Borrar`(IN p_id_detalle INT)
BEGIN
DELETE
FROM VentasDetalle
WHERE `IdDVEN`= p_id_detalle
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasDetalle_GetFallidas` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasDetalle_GetFallidas` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasDetalle_GetFallidas`(
in p_id int
)
BEGIN
select * from VentasDetalle WHERE IdDVEN = p_id;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasDetalle_Insertar` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasDetalle_Insertar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasDetalle_Insertar`(
IN p_id_detalle INT,
IN p_id_venta INT,
IN p_id_local INT,
IN p_articulo VARCHAR(50),
IN p_cantidad INT,
IN p_publico DOUBLE,
IN p_costo DOUBLE,
IN p_mayor DOUBLE,
IN p_forma_pago INT,
IN p_nro_cupon INT,
IN p_nro_factura INT,
IN p_id_empleado INT,
IN p_liquidado BIT,
IN p_devolucion BIT
)
BEGIN
INSERT INTO VentasDetalle(`IdDVEN`, `IdVentaDVEN`, `IdLocalDVEN`, `IdArticuloDVEN`, `CantidadDVEN`, `PrecioPublicoDVEN`,
`PrecioCostoDVEN`, `PrecioMayorDVEN`, `IdFormaPagoDVEN`, `NroCuponDVEN`, `NroFacturaDVEN`, 
`IdEmpleadoDVEN`, `LiquidadoDVEN`, `DevolucionDVEN`)
VALUES(p_id_detalle, p_id_venta, p_id_local, p_articulo, p_cantidad, p_publico, p_costo, p_mayor, p_forma_pago, 
p_nro_cupon, p_nro_factura, p_id_empleado, p_liquidado, p_devolucion)
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasDetalle_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasDetalle_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasDetalle_Listar`()
BEGIN
select *
FROM VentasDetalle
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasFallidas_Borrar` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasFallidas_Borrar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasFallidas_Borrar`(
in p_id int,
in p_accion varchar(20)
)
BEGIN
DELETE FROM VentasFallidas
where Id = p_id and Accion = p_accion
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasFallidas_BorrarByAccion` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasFallidas_BorrarByAccion` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasFallidas_BorrarByAccion`(
in p_accion varchar(20)
)
BEGIN
DELETE FROM VentasFallidas
where Accion = p_accion
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasFallidas_Existe` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasFallidas_Existe` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasFallidas_Existe`(
IN p_id int,
in p_accion varchar(20)
)
BEGIN
select * from VentasFallidas
where Id = p_id and Accion = p_accion
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasFallidas_Get` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasFallidas_Get` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasFallidas_Get`(
in p_accion varchar(20)
)
BEGIN
select * from VentasFallidas
where Accion = p_accion
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasFallidas_GetByAccion` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasFallidas_GetByAccion` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasFallidas_GetByAccion`(
in p_accion varchar(20)
)
BEGIN
select * from VentasFallidas
where Accion = p_accion
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasFallidas_Insertar` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasFallidas_Insertar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasFallidas_Insertar`(
IN p_id int(10),
in p_accion varchar(20)
)
BEGIN
INSERT INTO VentasFallidas(Id, Accion)
VALUES(p_id, p_accion)
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasFallidas_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasFallidas_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasFallidas_Listar`(
in p_id int
)
BEGIN
select * from Ventas where IdVentaVEN = p_id;
select * from VentasDetalle WHERE IdVentaDVEN = p_id;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasHistoricas_Insertar` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasHistoricas_Insertar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasHistoricas_Insertar`(
IN p_fecha date,
IN p_nombre varchar(50),
IN p_forma varchar(50),
IN p_total_publico decimal,
IN p_total_costo decimal,
IN p_prendas INT
)
BEGIN
INSERT INTO VentasH(`Fecha`, `NombreLocal`, `FormaPago`, `TotalPublico`, `TotalCosto`, `Prendas`)
VALUES(p_fecha, p_nombre, p_forma, p_total_publico, p_total_costo, p_prendas)
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasH_Insertar` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasH_Insertar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasH_Insertar`(IN p_fecha_hasta VARCHAR(50))
BEGIN
  DECLARE done INT DEFAULT 0;
  DECLARE registros INT DEFAULT 0;
  DECLARE fecha_max DATE;
  DECLARE nombre_local VARCHAR(50);
  DECLARE cur1 CURSOR FOR SELECT `NombreLOC` FROM `ncsoftwa_re`.`Locales` WHERE NombreLOC<>'Entradas' AND NombreLOC<>'Salidas';
  DECLARE CONTINUE HANDLER FOR SQLSTATE '02000' SET done = 1;
  
	DROP TEMPORARY TABLE IF EXISTS RegistrosTemp;
	CREATE TEMPORARY TABLE `RegistrosTemp` (`Cantidad` INT);  
  
  OPEN cur1;
  REPEAT 
    FETCH cur1 INTO nombre_local;
    IF NOT done THEN
		SET fecha_max = (SELECT MAX(Fecha) FROM VentasH WHERE NombreLocal = nombre_local);
		IF fecha_max IS NULL THEN
			SET registros = registros + (SELECT COUNT(Fecha) FROM VentasDiariasCons WHERE NombreLOC = nombre_local);	
			INSERT INTO VentasH (Fecha, NombreLocal, FormaPago, TotalPublico, TotalCosto, Prendas ) 
			SELECT Fecha, NombreLOC, DescripcionFOR, TotalPublico, TotalCosto, Prendas 
			FROM VentasDiariasCons
			WHERE Fecha<p_fecha_hasta and NombreLOC = nombre_local;			
		ELSE			
			SET registros = registros + (SELECT COUNT(Fecha) FROM VentasDiariasCons WHERE Fecha>fecha_max AND NombreLOC = nombre_local);	
			INSERT INTO VentasH (Fecha, NombreLocal, FormaPago, TotalPublico, TotalCosto, Prendas ) 
			SELECT Fecha, NombreLOC, DescripcionFOR, TotalPublico, TotalCosto, Prendas 
			FROM VentasDiariasCons
			WHERE Fecha>fecha_max AND Fecha<p_fecha_hasta and NombreLOC = nombre_local;	
		END IF;
    END IF;
  UNTIL done END REPEAT; 		
  CLOSE cur1;
  INSERT INTO RegistrosTemp (Cantidad) VALUES(registros);
  SELECT * FROM RegistrosTemp;
END */$$
DELIMITER ;

/* Procedure structure for procedure `VentasPesosCons_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `VentasPesosCons_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `VentasPesosCons_Listar`(
in p_locales varchar(200),
in p_forma int(2),
in p_fechaDesde varchar(50),
IN p_fechaHasta VARCHAR(50)
)
BEGIN
  DECLARE locales VARCHAR(50);
  DECLARE forma INT(2);
  DECLARE fechaDesde VARCHAR(50);
  DECLARE fechaHasta VARCHAR(50);
  SET locales = p_locales;
  SET forma = p_forma;
  SET fechaDesde = p_fechaDesde;
  SET fechaHasta = p_fechaHasta;
  
if forma =99 then
	SET @QUERY = CONCAT("SELECT 
	NombreLOC AS Local, SUM(Venta) AS Venta, SUM(Costo) AS Costo, SUM(Prendas) AS Prendas, 
	(SUM(Venta) - SUM(Costo)) AS 'Utilidad bruta', (((SUM(Venta)/SUM(Costo))-1)*100) AS 'Valor agregado'
	FROM VentasPesosCons 
	WHERE (",locales,") AND (FechaVEN >='",fechaDesde,"' AND FechaVEN <'",fechaHasta,"')
	GROUP BY NombreLOC");
else
	SET @QUERY = CONCAT("SELECT 
	NombreLOC AS Local, SUM(Venta) AS Venta, SUM(Costo) AS Costo, SUM(Prendas) AS Prendas, 
	(SUM(Venta) - SUM(Costo)) AS 'Utilidad bruta', (((SUM(Venta)/SUM(Costo))-1)*100) AS 'Valor agregado'
	FROM VentasPesosCons 
	WHERE (",locales,") AND (FechaVEN >='",fechaDesde,"' AND FechaVEN <'",fechaHasta,"') AND IdFormaPagoDVEN =",p_forma,"
	GROUP BY NombreLOC");
END IF;
  
  PREPARE smpt FROM @QUERY;
  EXECUTE smpt;
  
END */$$
DELIMITER ;

/* Procedure structure for procedure `Ventas_Actualizar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Ventas_Actualizar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Ventas_Actualizar`(
in p_id int,
IN p_id_pc INT,
IN p_fecha datetime,
in p_cliente int
)
BEGIN
update Ventas set IdPCVEN = p_id_pc, FechaVEN = p_fecha, IdClienteVEN = p_cliente
where `IdVentaVEN` = p_id
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Ventas_Arqueo` */

/*!50003 DROP PROCEDURE IF EXISTS  `Ventas_Arqueo` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Ventas_Arqueo`(
in p_fecha_desde varchar(50),
IN p_fecha_hasta VARCHAR(50),
in p_pc int(11)
)
BEGIN
SELECT * FROM Ventas
WHERE (`FechaVEN` >= p_fecha_desde AND `FechaVEN` < p_fecha_hasta) AND `IdPCVEN` = p_pc
;
SELECT * FROM VentasArqueoCons
WHERE (`FechaVEN` >= p_fecha_desde AND `FechaVEN` < p_fecha_hasta) AND `IdPCVEN` = p_pc
;
SELECT `ImporteFONP` FROM FondoCaja
WHERE `FechaFONP` = (SELECT MAX(`FechaFONP`) FROM FondoCaja WHERE `FechaFONP` < p_fecha_desde)
AND `IdPcFONP` = p_pc
;
SELECT `ImporteFONP` FROM FondoCaja
WHERE `FechaFONP` = p_fecha_desde
AND `IdPcFONP` = p_pc
;
SELECT * FROM `TesoreriaMovimientos`
WHERE `FechaTESM` = p_fecha_desde
AND `IdPcTESM` = p_pc
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Ventas_Borrar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Ventas_Borrar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Ventas_Borrar`(IN p_id INT)
BEGIN
DELETE
FROM Ventas
WHERE `IdVentaVEN` = p_id
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Ventas_Foraneos` */

/*!50003 DROP PROCEDURE IF EXISTS  `Ventas_Foraneos` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Ventas_Foraneos`()
BEGIN
select * FROM Articulos;
SELECT * FROM Clientes;
SELECT * FROM FormasPago;
SELECT * FROM Locales
	WHERE NombreLOC NOT LIKE 'Entradas' OR NombreLOC NOT LIKE 'Salidas'
	ORDER BY IdLocalLOC;
SELECT * FROM Pc;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Ventas_GetByPK` */

/*!50003 DROP PROCEDURE IF EXISTS  `Ventas_GetByPK` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Ventas_GetByPK`(IN p_id INT)
BEGIN
select count(IdVentaVEN)
FROM Ventas
WHERE `IdVentaVEN` = p_id
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Ventas_Historicas` */

/*!50003 DROP PROCEDURE IF EXISTS  `Ventas_Historicas` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Ventas_Historicas`()
BEGIN
select *
FROM VentasH
order by Fecha
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Ventas_Insertar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Ventas_Insertar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Ventas_Insertar`(
IN p_id INT,
IN p_id_pc INT,
IN p_fecha DATETIME,
IN p_cliente INT
)
BEGIN
INSERT INTO Ventas(`IdVentaVEN`, `IdPCVEN`, `FechaVEN`, `IdClienteVEN`)
VALUES(p_id, p_id_pc, p_fecha, p_cliente)
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Ventas_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Ventas_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Ventas_Listar`(
)
BEGIN
select * from Ventas;
select * from VentasDetalle;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Ventas_Schema` */

/*!50003 DROP PROCEDURE IF EXISTS  `Ventas_Schema` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Ventas_Schema`(
in p_id int
)
BEGIN
select * from Ventas where IdVentaVEN = p_id;
select * from VentasDetalle WHERE IdVentaDVEN = p_id;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Web_Articulo` */

/*!50003 DROP PROCEDURE IF EXISTS  `Web_Articulo` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Web_Articulo`(IN p_local INT, IN p_articulo VARCHAR(10))
BEGIN
	DECLARE articulo_left VARCHAR(10);
	SET articulo_left = LEFT(p_articulo,5);
	SELECT IdArticuloART, DescripcionWebART, PrecioPublicoART, ImagenART, ImagenBackART, ImagenColorART, HexCOL, DescripcionCOL, TalleART, CantidadSTK
	FROM Web_StockCons
	WHERE ((CantidadSTK>0 AND IdArticuloART LIKE CONCAT(articulo_left,'%') AND IdLocalSTK = p_local) 
	OR (IdArticuloART LIKE CONCAT(articulo_left,'%') AND IdLocalSTK = p_local AND ImagenART<>''))
	ORDER BY DescripcionCOL ASC;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Web_Articulos_by_Item` */

/*!50003 DROP PROCEDURE IF EXISTS  `Web_Articulos_by_Item` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Web_Articulos_by_Item`(IN p_local INT, IN p_idItem INT)
BEGIN 
	SELECT IdArticuloART, DescripcionWebART, PrecioPublicoART, ImagenART, IdLocalSTK 
	FROM Web_ArticulosByItemsCons
	WHERE IdLocalSTK = p_local AND IdItemART = p_idItem;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Web_Articulos_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Web_Articulos_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Web_Articulos_Listar`(
IN p_articulo INT,
IN p_local INT
)
BEGIN
SELECT COUNT(*) FROM Web_ArticulosCons 
WHERE (CONCAT("IdArticuloART LIKE '", p_articulo,"%'") AND IdLocalLOC = p_local)
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Web_Articulo_beta` */

/*!50003 DROP PROCEDURE IF EXISTS  `Web_Articulo_beta` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Web_Articulo_beta`(IN p_local INT, IN p_articulo VARCHAR(10))
BEGIN
	DECLARE articuloLeft VARCHAR(10);
	DECLARE tieneImagenColor INT(2);
	DECLARE tieneTalle VARCHAR(3);
	
	SET articuloLeft = LEFT(p_articulo,5);
	DROP TEMPORARY TABLE IF EXISTS ArticuloTemp;
	CREATE TEMPORARY TABLE `ArticuloTemp` (
	`IdArticuloART` VARCHAR(50) NOT NULL,
	`DescripcionWebART` VARCHAR(50) DEFAULT NULL,
	`PrecioPublicoART` DECIMAL(19,0) DEFAULT NULL,
	`ImagenART` VARCHAR(20) DEFAULT NULL,
	`ImagenBackART` VARCHAR(20) DEFAULT NULL,
	`ImagenColorART` VARCHAR(20) DEFAULT NULL,		
	`HexCOL` VARCHAR(20) DEFAULT NULL,		
	`DescripcionCOL` VARCHAR(50) DEFAULT NULL,	
	`TalleART` VARCHAR(2) DEFAULT NULL,	
	`CantidadSTK` INT(11) DEFAULT NULL
	);  	
	INSERT INTO ArticuloTemp (IdArticuloART, DescripcionWebART, PrecioPublicoART, ImagenART, ImagenBackART, ImagenColorART, HexCOL, 
			DescripcionCOL, TalleART, CantidadSTK) 
		SELECT IdArticuloART, DescripcionWebART, PrecioPublicoART, ImagenART, 
				ImagenBackART, ImagenColorART, HexCOL, DescripcionCOL, TalleART, CantidadSTK
		FROM Web_StockCons
		WHERE ((CantidadSTK>0 AND IdArticuloART LIKE CONCAT(articuloLeft,'%') AND IdLocalSTK = p_local))
		ORDER BY DescripcionCOL ASC;	
	SET tieneImagenColor = (SELECT COUNT(*) FROM ArticuloTemp WHERE ImagenColorART IS NOT NULL);
	IF tieneImagenColor = 0 THEN
		SELECT * FROM ArticuloTemp;
	ELSE
		SELECT * FROM ArticuloTemp WHERE ImagenColorART IS NOT NULL;
	END IF;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Web_Items_Listar` */

/*!50003 DROP PROCEDURE IF EXISTS  `Web_Items_Listar` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Web_Items_Listar`(
IN p_id INT
)
BEGIN
SELECT IdItemITE, DescripcionWebITE, IdLocalLOC FROM Web_ArticulosItemsCons
WHERE IdLocalLOC = p_id
GROUP BY DescripcionWebITE
ORDER BY DescripcionWebITE ASC
;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Web_Lo_Nuevo` */

/*!50003 DROP PROCEDURE IF EXISTS  `Web_Lo_Nuevo` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Web_Lo_Nuevo`(IN p_local INT)
BEGIN
  SELECT * FROM Web_LoNuevoCons WHERE IdLocalSTK = p_local;
END */$$
DELIMITER ;

/* Procedure structure for procedure `Web_Lo_Nuevo_Copia` */

/*!50003 DROP PROCEDURE IF EXISTS  `Web_Lo_Nuevo_Copia` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Web_Lo_Nuevo_Copia`(IN p_local INT)
BEGIN
  DECLARE done INT DEFAULT 0;
  DECLARE codigo_articulo VARCHAR(16);
  DECLARE codigo_5 VARCHAR(5);
   DECLARE codigo_7 VARCHAR(7);
  DECLARE imagen_color VARCHAR(16);
  DECLARE erroneos INT DEFAULT 0;
  DECLARE tiene_stock INT DEFAULT 0;
  DECLARE cur1 CURSOR FOR SELECT IdArticuloART, ImagenColorART FROM Articulos WHERE ImagenART<>'' AND NuevoART = 1 AND ActivoWebART = 1;
  DECLARE CONTINUE HANDLER FOR SQLSTATE '02000' SET done = 1;
  
  DROP TEMPORARY TABLE IF EXISTS ArticulosTemp;
  CREATE TEMPORARY TABLE `ArticulosTemp` (
    `IdArticuloART` VARCHAR(50) NOT NULL,
    `DescripcionART` VARCHAR(50) DEFAULT NULL,
    `DescripcionWebART` VARCHAR(50) DEFAULT NULL,
    `PrecioPublicoART` DECIMAL(19,0) DEFAULT NULL,
    `ImagenART` VARCHAR(20) DEFAULT NULL,
	`IdLocalSTK` INT(2) DEFAULT NULL    
  );  
  
  OPEN cur1;
  REPEAT
    FETCH cur1 INTO codigo_articulo, imagen_color;
    IF NOT done THEN
	IF imagen_color<>'' THEN
		SET codigo_7 = LEFT(codigo_articulo,7);
		SET tiene_stock=(SELECT COUNT(IdArticuloART) FROM Web_StockCons
							WHERE (IdArticuloART LIKE CONCAT(codigo_7,'%')
							AND CantidadSTK>0 AND IdLocalSTK = p_local));
		IF tiene_stock>0 THEN	
			INSERT INTO ArticulosTemp (IdArticuloART, DescripcionART, DescripcionWebART, PrecioPublicoART, ImagenART, IdLocalSTK ) 
			SELECT IdArticuloART, DescripcionART, DescripcionWebART, PrecioPublicoART, ImagenART, IdLocalSTK 
			FROM Web_StockCons
			WHERE IdArticuloART LIKE codigo_articulo AND IdLocalSTK = p_local AND ActivoWebITE = 1;
		END IF;
	ELSE
		SET codigo_5 = LEFT(codigo_articulo,5);
		SET erroneos=(SELECT COUNT(IdArticuloART) FROM Articulos
			WHERE IdArticuloART LIKE CONCAT(codigo_5,'%' AND ImagenColorART<>''));
		IF erroneos=0 THEN
			SET tiene_stock=(SELECT COUNT(IdArticuloART) FROM Web_StockCons
								WHERE (IdArticuloART LIKE CONCAT(codigo_5,'%')
								AND ImagenColorART ='' AND CantidadSTK>0
								AND IdLocalSTK = p_local));
			IF tiene_stock>0 THEN		
				INSERT INTO ArticulosTemp (IdArticuloART, DescripcionART, DescripcionWebART, PrecioPublicoART, ImagenART, IdLocalSTK ) 
				SELECT IdArticuloART, DescripcionART, DescripcionWebART, PrecioPublicoART, ImagenART, IdLocalSTK 
				FROM Web_StockCons
				WHERE IdArticuloART LIKE codigo_articulo AND IdLocalSTK = p_local  AND ActivoWebITE = 1;
			END IF;		
		END IF;
	END IF;
    END IF;
  UNTIL done END REPEAT;
  CLOSE cur1;
  SELECT * FROM ArticulosTemp;
END */$$
DELIMITER ;

/*Table structure for table `arqueocons` */

DROP TABLE IF EXISTS `arqueocons`;

/*!50001 DROP VIEW IF EXISTS `arqueocons` */;
/*!50001 DROP TABLE IF EXISTS `arqueocons` */;

/*!50001 CREATE TABLE  `arqueocons`(
 `IdVentaVEN` int(11) ,
 `IdPCVEN` int(11) ,
 `FechaVEN` datetime ,
 `IdClienteVEN` int(11) ,
 `IdDVEN` int(11) ,
 `IdVentaDVEN` int(11) ,
 `IdArticuloDVEN` varchar(50) ,
 `DescripcionDVEN` varchar(50) ,
 `CantidadDVEN` int(11) ,
 `PrecioPublicoDVEN` double ,
 `PrecioCostoDVEN` double ,
 `PrecioMayorDVEN` double ,
 `IdFormaPagoDVEN` int(11) ,
 `NroCuponDVEN` int(11) ,
 `NroFacturaDVEN` int(11) ,
 `IdEmpleadoDVEN` int(11) ,
 `LiquidadoDVEN` bit(1) ,
 `EsperaDVEN` bit(1) ,
 `DevolucionDVEN` smallint(1) ,
 `DescripcionART` varchar(55) ,
 `DescripcionFOR` varchar(50) 
)*/;

/*Table structure for table `fondocajacons` */

DROP TABLE IF EXISTS `fondocajacons`;

/*!50001 DROP VIEW IF EXISTS `fondocajacons` */;
/*!50001 DROP TABLE IF EXISTS `fondocajacons` */;

/*!50001 CREATE TABLE  `fondocajacons`(
 `FechaFONP` datetime ,
 `IdPcFONP` int(11) ,
 `IdLocalLOC` int(11) ,
 `NombreLOC` varchar(50) ,
 `Detalle` varchar(50) ,
 `ImporteFONP` double 
)*/;

/*Table structure for table `localescons` */

DROP TABLE IF EXISTS `localescons`;

/*!50001 DROP VIEW IF EXISTS `localescons` */;
/*!50001 DROP TABLE IF EXISTS `localescons` */;

/*!50001 CREATE TABLE  `localescons`(
 `IdLocalLOC` int(11) 
)*/;

/*Table structure for table `stockcons` */

DROP TABLE IF EXISTS `stockcons`;

/*!50001 DROP VIEW IF EXISTS `stockcons` */;
/*!50001 DROP TABLE IF EXISTS `stockcons` */;

/*!50001 CREATE TABLE  `stockcons`(
 `Articulo` varchar(50) ,
 `Descripcion` varchar(55) ,
 `ActivoWebART` int(1) ,
 `ImagenART` varchar(20) ,
 `Cantidad` int(11) ,
 `IdProveedorPRO` int(11) ,
 `Proveedor` varchar(50) ,
 `NombreLOC` varchar(50) ,
 `IdLocalSTK` int(11) 
)*/;

/*Table structure for table `stockmovcons` */

DROP TABLE IF EXISTS `stockmovcons`;

/*!50001 DROP VIEW IF EXISTS `stockmovcons` */;
/*!50001 DROP TABLE IF EXISTS `stockmovcons` */;

/*!50001 CREATE TABLE  `stockmovcons`(
 `FechaMSTK` datetime ,
 `OrigenMSTK` int(11) ,
 `DestinoMSTK` int(11) ,
 `CompensaMSTK` tinyint(1) ,
 `IdMSTKD` int(11) ,
 `IdMovMSTKD` int(11) ,
 `IdArticuloMSTKD` varchar(50) ,
 `DescripcionART` varchar(55) ,
 `CantidadMSTKD` int(11) ,
 `CompensaMSTKD` tinyint(1) ,
 `OrigenMSTKD` tinyint(2) ,
 `DestinoMSTKD` tinyint(2) 
)*/;

/*Table structure for table `ventasarqueocons` */

DROP TABLE IF EXISTS `ventasarqueocons`;

/*!50001 DROP VIEW IF EXISTS `ventasarqueocons` */;
/*!50001 DROP TABLE IF EXISTS `ventasarqueocons` */;

/*!50001 CREATE TABLE  `ventasarqueocons`(
 `IdPCVEN` int(11) ,
 `FechaVEN` datetime ,
 `IdDVEN` int(11) ,
 `IdVentaDVEN` int(11) ,
 `IdArticuloDVEN` varchar(50) ,
 `DescripcionDVEN` varchar(50) ,
 `CantidadDVEN` int(11) ,
 `PrecioPublicoDVEN` double ,
 `PrecioCostoDVEN` double ,
 `PrecioMayorDVEN` double ,
 `IdFormaPagoDVEN` int(11) ,
 `NroCuponDVEN` int(11) ,
 `NroFacturaDVEN` int(11) ,
 `IdEmpleadoDVEN` int(11) ,
 `LiquidadoDVEN` bit(1) ,
 `EsperaDVEN` bit(1) ,
 `DevolucionDVEN` smallint(1) ,
 `IdLocalDVEN` int(3) 
)*/;

/*Table structure for table `ventasdetallecons` */

DROP TABLE IF EXISTS `ventasdetallecons`;

/*!50001 DROP VIEW IF EXISTS `ventasdetallecons` */;
/*!50001 DROP TABLE IF EXISTS `ventasdetallecons` */;

/*!50001 CREATE TABLE  `ventasdetallecons`(
 `IdLocalLOC` int(11) ,
 `NombreLOC` varchar(50) ,
 `FechaVEN` datetime ,
 `IdDVEN` int(11) ,
 `IdArticuloDVEN` varchar(50) ,
 `DescripcionART` varchar(55) ,
 `CantidadDVEN` int(11) ,
 `PrecioPublicoDVEN` double ,
 `IdFormaPagoFOR` int(11) ,
 `DescripcionFOR` varchar(50) 
)*/;

/*Table structure for table `ventasdiariascons` */

DROP TABLE IF EXISTS `ventasdiariascons`;

/*!50001 DROP VIEW IF EXISTS `ventasdiariascons` */;
/*!50001 DROP TABLE IF EXISTS `ventasdiariascons` */;

/*!50001 CREATE TABLE  `ventasdiariascons`(
 `Fecha` varchar(10) ,
 `NombreLOC` varchar(50) ,
 `DescripcionFOR` varchar(50) ,
 `TotalPublico` double ,
 `TotalCosto` double ,
 `Prendas` decimal(32,0) 
)*/;

/*Table structure for table `ventaspesoscons` */

DROP TABLE IF EXISTS `ventaspesoscons`;

/*!50001 DROP VIEW IF EXISTS `ventaspesoscons` */;
/*!50001 DROP TABLE IF EXISTS `ventaspesoscons` */;

/*!50001 CREATE TABLE  `ventaspesoscons`(
 `IdLocalLOC` int(11) ,
 `NombreLOC` varchar(50) ,
 `FechaVEN` datetime ,
 `IdFormaPagoDVEN` int(11) ,
 `Prendas` int(11) ,
 `Costo` double ,
 `Venta` double 
)*/;

/*Table structure for table `ventaspesoscons2` */

DROP TABLE IF EXISTS `ventaspesoscons2`;

/*!50001 DROP VIEW IF EXISTS `ventaspesoscons2` */;
/*!50001 DROP TABLE IF EXISTS `ventaspesoscons2` */;

/*!50001 CREATE TABLE  `ventaspesoscons2`(
 `IdLocalLOC` int(11) ,
 `NombreLOC` varchar(50) ,
 `FechaVEN` datetime ,
 `CantidadDVEN` int(11) ,
 `PrecioPublicoDVEN` double ,
 `PrecioCostoDVEN` double ,
 `IdFormaPagoDVEN` int(11) 
)*/;

/*View structure for view arqueocons */

/*!50001 DROP TABLE IF EXISTS `arqueocons` */;
/*!50001 DROP VIEW IF EXISTS `arqueocons` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `arqueocons` AS select `ventas`.`IdVentaVEN` AS `IdVentaVEN`,`ventas`.`IdPCVEN` AS `IdPCVEN`,`ventas`.`FechaVEN` AS `FechaVEN`,`ventas`.`IdClienteVEN` AS `IdClienteVEN`,`ventasdetalle`.`IdDVEN` AS `IdDVEN`,`ventasdetalle`.`IdVentaDVEN` AS `IdVentaDVEN`,`ventasdetalle`.`IdArticuloDVEN` AS `IdArticuloDVEN`,`ventasdetalle`.`DescripcionDVEN` AS `DescripcionDVEN`,`ventasdetalle`.`CantidadDVEN` AS `CantidadDVEN`,`ventasdetalle`.`PrecioPublicoDVEN` AS `PrecioPublicoDVEN`,`ventasdetalle`.`PrecioCostoDVEN` AS `PrecioCostoDVEN`,`ventasdetalle`.`PrecioMayorDVEN` AS `PrecioMayorDVEN`,`ventasdetalle`.`IdFormaPagoDVEN` AS `IdFormaPagoDVEN`,`ventasdetalle`.`NroCuponDVEN` AS `NroCuponDVEN`,`ventasdetalle`.`NroFacturaDVEN` AS `NroFacturaDVEN`,`ventasdetalle`.`IdEmpleadoDVEN` AS `IdEmpleadoDVEN`,`ventasdetalle`.`LiquidadoDVEN` AS `LiquidadoDVEN`,`ventasdetalle`.`EsperaDVEN` AS `EsperaDVEN`,`ventasdetalle`.`DevolucionDVEN` AS `DevolucionDVEN`,`articulos`.`DescripcionART` AS `DescripcionART`,`formaspago`.`DescripcionFOR` AS `DescripcionFOR` from (((`ventasdetalle` join `ventas` on((`ventasdetalle`.`IdVentaDVEN` = `ventas`.`IdVentaVEN`))) join `articulos` on((`ventasdetalle`.`IdArticuloDVEN` = `articulos`.`IdArticuloART`))) join `formaspago` on((`ventasdetalle`.`IdFormaPagoDVEN` = `formaspago`.`IdFormaPagoFOR`))) */;

/*View structure for view fondocajacons */

/*!50001 DROP TABLE IF EXISTS `fondocajacons` */;
/*!50001 DROP VIEW IF EXISTS `fondocajacons` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `fondocajacons` AS select `fondocaja`.`FechaFONP` AS `FechaFONP`,`fondocaja`.`IdPcFONP` AS `IdPcFONP`,`locales`.`IdLocalLOC` AS `IdLocalLOC`,`locales`.`NombreLOC` AS `NombreLOC`,`pc`.`Detalle` AS `Detalle`,`fondocaja`.`ImporteFONP` AS `ImporteFONP` from ((`pc` join `locales` on((`pc`.`IdLocalPC` = `locales`.`IdLocalLOC`))) join `fondocaja` on((`fondocaja`.`IdPcFONP` = `pc`.`IdPC`))) order by `fondocaja`.`FechaFONP` */;

/*View structure for view localescons */

/*!50001 DROP TABLE IF EXISTS `localescons` */;
/*!50001 DROP VIEW IF EXISTS `localescons` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `localescons` AS select `locales`.`IdLocalLOC` AS `IdLocalLOC` from `locales` where ((`locales`.`IdLocalLOC` <> 1) and (`locales`.`IdLocalLOC` <> 2)) */;

/*View structure for view stockcons */

/*!50001 DROP TABLE IF EXISTS `stockcons` */;
/*!50001 DROP VIEW IF EXISTS `stockcons` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `stockcons` AS select `stock`.`IdArticuloSTK` AS `Articulo`,`articulos`.`DescripcionART` AS `Descripcion`,`articulos`.`ActivoWebART` AS `ActivoWebART`,`articulos`.`ImagenART` AS `ImagenART`,`stock`.`CantidadSTK` AS `Cantidad`,`proveedores`.`IdProveedorPRO` AS `IdProveedorPRO`,`proveedores`.`RazonSocialPRO` AS `Proveedor`,`locales`.`NombreLOC` AS `NombreLOC`,`stock`.`IdLocalSTK` AS `IdLocalSTK` from (((`stock` join `articulos` on((`stock`.`IdArticuloSTK` = `articulos`.`IdArticuloART`))) join `proveedores` on((`articulos`.`IdProveedorART` = `proveedores`.`IdProveedorPRO`))) join `locales` on((`stock`.`IdLocalSTK` = `locales`.`IdLocalLOC`))) where (`stock`.`CantidadSTK` <> 0) */;

/*View structure for view stockmovcons */

/*!50001 DROP TABLE IF EXISTS `stockmovcons` */;
/*!50001 DROP VIEW IF EXISTS `stockmovcons` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `stockmovcons` AS select `stockmov`.`FechaMSTK` AS `FechaMSTK`,`stockmov`.`OrigenMSTK` AS `OrigenMSTK`,`stockmov`.`DestinoMSTK` AS `DestinoMSTK`,`stockmov`.`CompensaMSTK` AS `CompensaMSTK`,`stockmovdetalle`.`IdMSTKD` AS `IdMSTKD`,`stockmovdetalle`.`IdMovMSTKD` AS `IdMovMSTKD`,`stockmovdetalle`.`IdArticuloMSTKD` AS `IdArticuloMSTKD`,`articulos`.`DescripcionART` AS `DescripcionART`,`stockmovdetalle`.`CantidadMSTKD` AS `CantidadMSTKD`,`stockmovdetalle`.`CompensaMSTKD` AS `CompensaMSTKD`,`stockmovdetalle`.`OrigenMSTKD` AS `OrigenMSTKD`,`stockmovdetalle`.`DestinoMSTKD` AS `DestinoMSTKD` from ((`stockmovdetalle` join `stockmov` on((`stockmovdetalle`.`IdMovMSTKD` = `stockmov`.`IdMovMSTK`))) join `articulos` on((`stockmovdetalle`.`IdArticuloMSTKD` = `articulos`.`IdArticuloART`))) */;

/*View structure for view ventasarqueocons */

/*!50001 DROP TABLE IF EXISTS `ventasarqueocons` */;
/*!50001 DROP VIEW IF EXISTS `ventasarqueocons` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `ventasarqueocons` AS select `ventas`.`IdPCVEN` AS `IdPCVEN`,`ventas`.`FechaVEN` AS `FechaVEN`,`ventasdetalle`.`IdDVEN` AS `IdDVEN`,`ventasdetalle`.`IdVentaDVEN` AS `IdVentaDVEN`,`ventasdetalle`.`IdArticuloDVEN` AS `IdArticuloDVEN`,`ventasdetalle`.`DescripcionDVEN` AS `DescripcionDVEN`,`ventasdetalle`.`CantidadDVEN` AS `CantidadDVEN`,`ventasdetalle`.`PrecioPublicoDVEN` AS `PrecioPublicoDVEN`,`ventasdetalle`.`PrecioCostoDVEN` AS `PrecioCostoDVEN`,`ventasdetalle`.`PrecioMayorDVEN` AS `PrecioMayorDVEN`,`ventasdetalle`.`IdFormaPagoDVEN` AS `IdFormaPagoDVEN`,`ventasdetalle`.`NroCuponDVEN` AS `NroCuponDVEN`,`ventasdetalle`.`NroFacturaDVEN` AS `NroFacturaDVEN`,`ventasdetalle`.`IdEmpleadoDVEN` AS `IdEmpleadoDVEN`,`ventasdetalle`.`LiquidadoDVEN` AS `LiquidadoDVEN`,`ventasdetalle`.`EsperaDVEN` AS `EsperaDVEN`,`ventasdetalle`.`DevolucionDVEN` AS `DevolucionDVEN`,`ventasdetalle`.`IdLocalDVEN` AS `IdLocalDVEN` from (`ventasdetalle` join `ventas` on((`ventasdetalle`.`IdVentaDVEN` = `ventas`.`IdVentaVEN`))) */;

/*View structure for view ventasdetallecons */

/*!50001 DROP TABLE IF EXISTS `ventasdetallecons` */;
/*!50001 DROP VIEW IF EXISTS `ventasdetallecons` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `ventasdetallecons` AS select `locales`.`IdLocalLOC` AS `IdLocalLOC`,`locales`.`NombreLOC` AS `NombreLOC`,`ventas`.`FechaVEN` AS `FechaVEN`,`ventasdetalle`.`IdDVEN` AS `IdDVEN`,`ventasdetalle`.`IdArticuloDVEN` AS `IdArticuloDVEN`,`articulos`.`DescripcionART` AS `DescripcionART`,`ventasdetalle`.`CantidadDVEN` AS `CantidadDVEN`,`ventasdetalle`.`PrecioPublicoDVEN` AS `PrecioPublicoDVEN`,`formaspago`.`IdFormaPagoFOR` AS `IdFormaPagoFOR`,`formaspago`.`DescripcionFOR` AS `DescripcionFOR` from (((((`ventasdetalle` join `ventas` on((`ventasdetalle`.`IdVentaDVEN` = `ventas`.`IdVentaVEN`))) join `pc` on((`ventas`.`IdPCVEN` = `pc`.`IdPC`))) join `locales` on((`pc`.`IdLocalPC` = `locales`.`IdLocalLOC`))) join `articulos` on((`ventasdetalle`.`IdArticuloDVEN` = `articulos`.`IdArticuloART`))) join `formaspago` on((`ventasdetalle`.`IdFormaPagoDVEN` = `formaspago`.`IdFormaPagoFOR`))) */;

/*View structure for view ventasdiariascons */

/*!50001 DROP TABLE IF EXISTS `ventasdiariascons` */;
/*!50001 DROP VIEW IF EXISTS `ventasdiariascons` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `ventasdiariascons` AS select date_format(`ventas`.`FechaVEN`,'%Y-%m-%d') AS `Fecha`,`locales`.`NombreLOC` AS `NombreLOC`,`formaspago`.`DescripcionFOR` AS `DescripcionFOR`,sum((`ventasdetalle`.`CantidadDVEN` * `ventasdetalle`.`PrecioPublicoDVEN`)) AS `TotalPublico`,sum((`ventasdetalle`.`CantidadDVEN` * `ventasdetalle`.`PrecioCostoDVEN`)) AS `TotalCosto`,sum(`ventasdetalle`.`CantidadDVEN`) AS `Prendas` from ((((`ventasdetalle` join `ventas` on((`ventasdetalle`.`IdVentaDVEN` = `ventas`.`IdVentaVEN`))) join `pc` on((`ventas`.`IdPCVEN` = `pc`.`IdPC`))) join `locales` on((`pc`.`IdLocalPC` = `locales`.`IdLocalLOC`))) join `formaspago` on((`ventasdetalle`.`IdFormaPagoDVEN` = `formaspago`.`IdFormaPagoFOR`))) where ((`locales`.`NombreLOC` <> 'Entradas') and (`locales`.`NombreLOC` <> 'Salidas')) group by date_format(`ventas`.`FechaVEN`,'%Y-%m-%d'),`locales`.`NombreLOC`,`formaspago`.`DescripcionFOR` order by date_format(`ventas`.`FechaVEN`,'%Y-%m-%d') */;

/*View structure for view ventaspesoscons */

/*!50001 DROP TABLE IF EXISTS `ventaspesoscons` */;
/*!50001 DROP VIEW IF EXISTS `ventaspesoscons` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `ventaspesoscons` AS select `locales`.`IdLocalLOC` AS `IdLocalLOC`,`locales`.`NombreLOC` AS `NombreLOC`,`ventas`.`FechaVEN` AS `FechaVEN`,`ventasdetalle`.`IdFormaPagoDVEN` AS `IdFormaPagoDVEN`,`ventasdetalle`.`CantidadDVEN` AS `Prendas`,(`ventasdetalle`.`PrecioCostoDVEN` * `ventasdetalle`.`CantidadDVEN`) AS `Costo`,(`ventasdetalle`.`PrecioPublicoDVEN` * `ventasdetalle`.`CantidadDVEN`) AS `Venta` from (((`ventasdetalle` join `ventas` on((`ventasdetalle`.`IdVentaDVEN` = `ventas`.`IdVentaVEN`))) join `pc` on((`ventas`.`IdPCVEN` = `pc`.`IdPC`))) join `locales` on((`pc`.`IdLocalPC` = `locales`.`IdLocalLOC`))) */;

/*View structure for view ventaspesoscons2 */

/*!50001 DROP TABLE IF EXISTS `ventaspesoscons2` */;
/*!50001 DROP VIEW IF EXISTS `ventaspesoscons2` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `ventaspesoscons2` AS select `locales`.`IdLocalLOC` AS `IdLocalLOC`,`locales`.`NombreLOC` AS `NombreLOC`,`ventas`.`FechaVEN` AS `FechaVEN`,`ventasdetalle`.`CantidadDVEN` AS `CantidadDVEN`,`ventasdetalle`.`PrecioPublicoDVEN` AS `PrecioPublicoDVEN`,`ventasdetalle`.`PrecioCostoDVEN` AS `PrecioCostoDVEN`,`ventasdetalle`.`IdFormaPagoDVEN` AS `IdFormaPagoDVEN` from (((`ventasdetalle` join `ventas` on((`ventasdetalle`.`IdVentaDVEN` = `ventas`.`IdVentaVEN`))) join `pc` on((`ventas`.`IdPCVEN` = `pc`.`IdPC`))) join `locales` on((`pc`.`IdLocalPC` = `locales`.`IdLocalLOC`))) */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
