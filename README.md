# Proyecto5Estrellas
## Micro-servicio con Api REST para manejo de Usuarios

### En este proyecto se construyó un micro-servicios, con un API tipo REST, la cual permite manejar la información de los Usuarios en una Base de Datos.

### El desarrollo tiene las siguientes características:
[-] Microservicio con un Api tipo REST (Proyecto tipo WebApi con C# y .NET 8)
[-] El Api REST expone 4 end-points, con los cuales se podrá Consultar / Crear / Actualizar / Eliminar la información de los Usuarios en la BD.
[-] Para la persistencia de la información de los Usuarios, se utiliza una BD no relacional, tipo MongoDB, con una colección (Usuarios).
[-] Para efectos de asegurar el acceso y el consumo del Api, se implementó la Autenticación con Json Web Token (JWT), por lo tanto, solo los usuarios autorizados podrán consumir el Api.
[-] Para evitar la sobrecarga y el abuso en el consumo del Servicio, se implementó un Rate-Limiter.
[-] Para garantizar una transición sin traumas hacia las futuras versiones del Api, se implementó Api-Versioning.
[-] El Api cuenta con dos end-points adicionales, que permiten validar su estado de salud (HealthChecks), incluyendo la BD (MongoDB).
[-] Igualmente se implementó un Reverse-Proxy a modo de Api Gateway, para garantizar, entre otras cosas, que si se cambia la ubicación del Servicio que gestiona los usuarios, el consumidor no se vea afectado.
[-] Para poderse desplegar en cualquiera de los proveedores de Nube, se contenerizó el Servicio con Docker.
[-] Dado que el Servicio está contenerizado, la funcionalidad se podría escalar horizontalmente, modificando el número de las instancias, según como lo requiera la demanda del momento. (esto requiere de componentes como el Kubernetes).

