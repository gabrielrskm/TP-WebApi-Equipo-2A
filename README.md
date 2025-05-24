Base de datos usada: CATALOGO_P3_DB

##Endpoints:
⁠Búsqueda de Producto:  
Listado Producto:      GET api/Producto
Alta Producto:         POST api/Producto
Busqueda Producto:     GET api/Producto/{id}
Eliminar producto:     DELETE api/Producto/{id}
Modificar Producto:    PATCH api/Producto/{id} --> (se puede modificar el producto de forma parcial)
Ejemplo:
```json
{
  "Nombre": "Camiseta deportiva",
  "Precio": 19.99,
  "IDMarca": 10,
  "Imagenes": [
    "https://ejemplo.com/imagen1.jpg",
    "https://ejemplo.com/imagen2.jpg"
  ]
}
```
Ejemplo:
Agregar imagenes:    POST api/Imagen/{id}
```json
{
  "Urls": [
    "https://ejemplo.com/imagen1.jpg",
    "https://ejemplo.com/imagen2.jpg"
  ]
}
```
NOTA: Agregar a PUERTO, DATABASE, USERID, PASSWORD a TP-WebApi-equipo-2A/Web.config
```
	<connectionStrings>
		<add name="DBConnection" connectionString="Server=localhost,PUERTO;Database=DATABASE;User Id=USERID;Password=PASSWORD;TrustServerCertificate=True;" providerName="System.Data.SqlClient"/>
	</connectionStrings>
```
