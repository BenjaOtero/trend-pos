DELETE FROM Ventas;
SELECT * FROM Ventas WHERE FechaVEN>'2014-08-15';
SELECT * FROM VentasDetalle WHERE IdDVEN = '633884506';
SELECT * FROM VentasDetalle WHERE IdVentaDVEN = '912538656';

SELECT
    `VentasDetalle`.`IdDVEN`
    , `VentasDetalle`.`IdVentaDVEN`
    , `Ventas`.`FechaVEN`
    , `VentasDetalle`.`CantidadDVEN`
    , `VentasDetalle`.`PrecioPublicoDVEN`
FROM
    `ncsoftwa_pruebas`.`VentasDetalle`
    INNER JOIN `ncsoftwa_pruebas`.`Ventas` 
        ON (`VentasDetalle`.`IdVentaDVEN` = `Ventas`.`IdVentaVEN`)
WHERE (`Ventas`.`FechaVEN` >'2014-06-18');

SELECT * FROM `tesoreriamovimientos` WHERE `FechaTESM`>='2015-03-01' AND `FechaTESM`<'2015-04-01'
AND IdPcTESM = 3 ORDER BY FechaTESM;

SELECT * FROM `FondoCaja` WHERE `FechaFONP`>'2014-08-14';