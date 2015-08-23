DELIMITER $$

USE `ncsoftwa_re`$$

DROP PROCEDURE IF EXISTS `Articulos_Precios`$$

CREATE DEFINER=`ncsoftwa_re`@`%` PROCEDURE `Articulos_Precios`()
BEGIN
	DECLARE done INT DEFAULT 0;
	DECLARE codigo_articulo VARCHAR(16);	  
	DECLARE precio_publico DECIMAL(19);
	DECLARE cur1 CURSOR FOR SELECT IdArticuloART, PrecioPublicoART FROM articulos;
	DECLARE CONTINUE HANDLER FOR SQLSTATE '02000' SET done = 1;
	OPEN cur1;
	  REPEAT
		FETCH cur1 INTO codigo_articulo, precio_publico;
		IF NOT done THEN
			UPDATE articulos SET PrecioMayorART = 1;
		END IF;
	  UNTIL done END REPEAT;
	  CLOSE cur1;
;
END$$

DELIMITER ;