/*
  DATOS DE DEMOSTRACIÓN - Tienda de Basti
  Para correr sobre la base que ya existe (por ejemplo la de Somee). Es idempotente: se puede ejecutar
  más de una vez y el resultado es el mismo.
  - Los 6 artículos originales se actualizan por Id (1, 2, 3, 4, 5 y 7).
  - Los artículos nuevos se insertan solo si su código todavía no existe.
  - Marca y categoría se buscan por nombre, así no depende de que los Id sean iguales en cada base.
  - Las imágenes son locales (Images/Articulos/...): hay que publicar el sitio con esas fotos.
  Todo corre en una transacción: si algo falla, no se aplica nada.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;   -- ante cualquier error, se deshace toda la transacción

BEGIN TRANSACTION;

-- Control: tienen que existir todas las marcas y categorías que se usan abajo
IF (SELECT COUNT(*) FROM MARCAS WHERE Descripcion IN ('Samsung', 'Apple', 'Sony', 'Huawei', 'Motorola')) <> 5
   OR (SELECT COUNT(*) FROM CATEGORIAS WHERE Descripcion IN ('Celulares', 'Televisores', 'Media', 'Audio')) <> 4
    THROW 50001, 'Faltan marcas o categorías en la base: no se aplicó ningún cambio.', 1;

-- 1) Artículos existentes: se actualizan por Id
IF NOT EXISTS (SELECT 1 FROM ARTICULOS WHERE Id = 1) PRINT 'Aviso: no existe el artículo con Id 1 (no se actualizó).';
UPDATE ARTICULOS SET
    Codigo = N'S01', Nombre = N'Samsung Galaxy S25 Ultra',
    Descripcion = N'Pantalla Dynamic AMOLED de 6,9 pulgadas, cámara principal de 200 MP y S Pen integrado. Procesador Snapdragon 8 Elite.',
    IdMarca = (SELECT Id FROM MARCAS WHERE Descripcion = N'Samsung'),
    IdCategoria = (SELECT Id FROM CATEGORIAS WHERE Descripcion = N'Celulares'),
    ImagenUrl = N'Images/Articulos/samsung-galaxy-s25-ultra.jpg', Precio = 2499999
WHERE Id = 1;

IF NOT EXISTS (SELECT 1 FROM ARTICULOS WHERE Id = 2) PRINT 'Aviso: no existe el artículo con Id 2 (no se actualizó).';
UPDATE ARTICULOS SET
    Codigo = N'S03', Nombre = N'Motorola Edge 60 Fusion',
    Descripcion = N'Pantalla pOLED curva de 6,67 pulgadas, cámara principal de 50 MP y protección IP68/IP69 contra agua y polvo.',
    IdMarca = (SELECT Id FROM MARCAS WHERE Descripcion = N'Motorola'),
    IdCategoria = (SELECT Id FROM CATEGORIAS WHERE Descripcion = N'Celulares'),
    ImagenUrl = N'Images/Articulos/motorola-edge-60-fusion.jpg', Precio = 799999
WHERE Id = 2;

IF NOT EXISTS (SELECT 1 FROM ARTICULOS WHERE Id = 3) PRINT 'Aviso: no existe el artículo con Id 3 (no se actualizó).';
UPDATE ARTICULOS SET
    Codigo = N'S99', Nombre = N'Sony PlayStation 4',
    Descripcion = N'Consola PlayStation 4 en color blanco con joystick inalámbrico DualShock 4. Compatible con juegos físicos y digitales.',
    IdMarca = (SELECT Id FROM MARCAS WHERE Descripcion = N'Sony'),
    IdCategoria = (SELECT Id FROM CATEGORIAS WHERE Descripcion = N'Media'),
    ImagenUrl = N'Images/Articulos/playstation-4.jpg', Precio = 549999
WHERE Id = 3;

IF NOT EXISTS (SELECT 1 FROM ARTICULOS WHERE Id = 4) PRINT 'Aviso: no existe el artículo con Id 4 (no se actualizó).';
UPDATE ARTICULOS SET
    Codigo = N'S56', Nombre = N'Smart TV Sony Bravia',
    Descripcion = N'Smart TV con resolución 4K HDR y Google TV integrado. Accedé a Netflix, YouTube y Prime Video desde una sola pantalla.',
    IdMarca = (SELECT Id FROM MARCAS WHERE Descripcion = N'Sony'),
    IdCategoria = (SELECT Id FROM CATEGORIAS WHERE Descripcion = N'Televisores'),
    ImagenUrl = N'Images/Articulos/televisor-sony-bravia.jpg', Precio = 1299999
WHERE Id = 4;

IF NOT EXISTS (SELECT 1 FROM ARTICULOS WHERE Id = 5) PRINT 'Aviso: no existe el artículo con Id 5 (no se actualizó).';
UPDATE ARTICULOS SET
    Codigo = N'A23', Nombre = N'Apple TV',
    Descripcion = N'Reproductor multimedia con tvOS para ver tus apps de streaming en el televisor. Incluye control remoto y conexión HDMI.',
    IdMarca = (SELECT Id FROM MARCAS WHERE Descripcion = N'Apple'),
    IdCategoria = (SELECT Id FROM CATEGORIAS WHERE Descripcion = N'Media'),
    ImagenUrl = N'Images/Articulos/apple-tv.jpg', Precio = 299999
WHERE Id = 5;

IF NOT EXISTS (SELECT 1 FROM ARTICULOS WHERE Id = 7) PRINT 'Aviso: no existe el artículo con Id 7 (no se actualizó).';
UPDATE ARTICULOS SET
    Codigo = N'S71', Nombre = N'Cafetera espresso',
    Descripcion = N'Cafetera espresso con portafiltro de acero inoxidable para preparar espresso y cappuccino en casa.',
    IdMarca = (SELECT Id FROM MARCAS WHERE Descripcion = N'Sony'),
    IdCategoria = (SELECT Id FROM CATEGORIAS WHERE Descripcion = N'Media'),
    ImagenUrl = N'Images/Articulos/cafetera-espresso.jpg', Precio = 459999
WHERE Id = 7;

-- 2) Artículos nuevos: se insertan solo si el código no existe
IF NOT EXISTS (SELECT 1 FROM ARTICULOS WHERE Codigo = N'H01')
    INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio)
    VALUES (N'H01', N'Huawei P20 Pro',
            N'Triple cámara Leica de hasta 40 MP, pantalla OLED de 6,1 pulgadas y batería de 4000 mAh con carga rápida.',
            (SELECT Id FROM MARCAS WHERE Descripcion = N'Huawei'), (SELECT Id FROM CATEGORIAS WHERE Descripcion = N'Celulares'),
            N'Images/Articulos/huawei-p20-pro.jpg', 399999);

IF NOT EXISTS (SELECT 1 FROM ARTICULOS WHERE Codigo = N'A24')
    INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio)
    VALUES (N'A24', N'Apple iPhone 15 Pro',
            N'Diseño de titanio, chip A17 Pro, cámara principal de 48 MP y conector USB-C. Pantalla Super Retina XDR de 6,1 pulgadas.',
            (SELECT Id FROM MARCAS WHERE Descripcion = N'Apple'), (SELECT Id FROM CATEGORIAS WHERE Descripcion = N'Celulares'),
            N'Images/Articulos/iphone-15-pro.jpg', 2199999);

IF NOT EXISTS (SELECT 1 FROM ARTICULOS WHERE Codigo = N'S12')
    INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio)
    VALUES (N'S12', N'Smart TV Samsung',
            N'Smart TV con sistema Tizen y resolución 4K. Incluye Samsung TV Plus con canales en vivo gratuitos y apps de streaming.',
            (SELECT Id FROM MARCAS WHERE Descripcion = N'Samsung'), (SELECT Id FROM CATEGORIAS WHERE Descripcion = N'Televisores'),
            N'Images/Articulos/smart-tv-samsung.jpg', 999999);

IF NOT EXISTS (SELECT 1 FROM ARTICULOS WHERE Codigo = N'S80')
    INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio)
    VALUES (N'S80', N'Auriculares Sony Bluetooth',
            N'Auriculares inalámbricos over-ear con almohadillas acolchadas, micrófono integrado para llamadas y batería de larga duración.',
            (SELECT Id FROM MARCAS WHERE Descripcion = N'Sony'), (SELECT Id FROM CATEGORIAS WHERE Descripcion = N'Audio'),
            N'Images/Articulos/auriculares-sony-bluetooth.jpg', 179999);

IF NOT EXISTS (SELECT 1 FROM ARTICULOS WHERE Codigo = N'A30')
    INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio)
    VALUES (N'A30', N'Apple AirPods Pro',
            N'Auriculares inalámbricos con cancelación activa de ruido, modo Ambiente y estuche de carga. Se vinculan al instante con iPhone.',
            (SELECT Id FROM MARCAS WHERE Descripcion = N'Apple'), (SELECT Id FROM CATEGORIAS WHERE Descripcion = N'Audio'),
            N'Images/Articulos/airpods-pro.jpg', 449999);

IF NOT EXISTS (SELECT 1 FROM ARTICULOS WHERE Codigo = N'S15')
    INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio)
    VALUES (N'S15', N'Samsung Galaxy Buds Live',
            N'Auriculares inalámbricos de diseño abierto con cancelación activa de ruido, estuche de carga compacto y conexión Bluetooth.',
            (SELECT Id FROM MARCAS WHERE Descripcion = N'Samsung'), (SELECT Id FROM CATEGORIAS WHERE Descripcion = N'Audio'),
            N'Images/Articulos/samsung-galaxy-buds-live.jpg', 149999);

COMMIT TRANSACTION;

PRINT 'Datos de demostración aplicados.';

SELECT A.Id, A.Codigo, A.Nombre, M.Descripcion AS Marca, C.Descripcion AS Categoria, A.Precio
FROM ARTICULOS A INNER JOIN MARCAS M ON A.IdMarca = M.Id INNER JOIN CATEGORIAS C ON A.IdCategoria = C.Id
ORDER BY C.Descripcion, M.Descripcion;
