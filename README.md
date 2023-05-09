# Prueba técnica de Backend para Atmira

## Índice

- [Descripción de la prueba](#descripcion)
- [Restricciones](#restricciones)
- [Endpoints](#endpoints)
- [Casos de prueba](#casos-de-prueba)
 
<a name="descripcion"></a>
### Descripción de la prueba
Exponer un endpoint que reciba un número de días entre 1 y 7 y que devuelva un listado en 
formato json con el top 3 de asteroides más grandes con potencial riesgo de impacto en el 
planeta Tierra entre el día de hoy y la fecha obtenida de sumar a la fecha de hoy el número de 
días introducido como parámetro.

*`Ejemplo de endpoint: /asteroids?days=3`*

El parámetro days es obligatorio. Si no se especifica, se debe devolver un error controlado 
indicando este hecho.

El parámetro days debe ser un valor incluido en 1 y 7. Si no se satisface, la API de la Nasa 
devuelve un error controlado.

Los datos se recogen de la siguiente API, deben transformarse y construir la respuesta con la 
estructura planteada más abajo. Start_date será el día actual y el parámetro end_date se 
calculará sumando al día actual el número de días incluido como parámetro.

*https://api.nasa.gov/neo/rest/v1/feed?start_date=2021-12-09&end_date=2021-12-12&api_key=DEMO_KEY*

Campos clave del servicio de la NASA para la obtención de resultados:
- "is_potentially_hazardous_asteroid" = true
- "estimated_diameter:kilometers: estimated_diameter_min" y 
"estimated_diameter:kilometers: estimated_diameter_max": Para calcular el tamaño medio.

Campos de respuesta del endpoint /asteroids, devolver json con:
- **nombre**: Obtenido de "name",
- **diametro**: Tamaño medio calculado
- **velocidad**: "close_approach_data:relative_velocity:kilometers_per_hour"
- **fecha**: "close_approach_data:close_approach_date"
- **planeta**: "close_approach_date:orbiting_body"

<a name="restricciones"></a>
### Restricciones

#### Tecnológicas 💻
 El planteamiento es se utilice las siguientes herramientas, pero no se está limitado a ellas.
- .Net CORE/NET5, Asp.net WebApi
- Test xUnit/NUnit/MsTest (a elegir) y Moq

#### Publicación 🚀
En git o en cualquier repositorio en el que podamos acceder.

#### Principios SOLID 🔒
Foco en estructura de código ordenado y buenas prácticas de programación.

<a name="endpoints"></a>
### Endpoints
#### Obtener asteroides potencialmente peligrosos
Este endpoint es un método HttpGet que permite obtener una lista de los tres asteroides más grandes
con potencial de riesgo de impacto en el planeta Tierra entre el día de hoy y una fecha específica 
en el futuro. 

La ruta relativa para este endpoint es */asteroids?days=xx*, siendo xx los días a añadirle la fecha,
teniendo que estar entre 1 y 7.

##### Detalles
El método GetAsteroids se encuentra en la clase de controlador AsteroidController.
En este método se realiza una petición a una API externa para obtener información sobre asteroides 
cercanos a la Tierra, se procesa la respuesta para obtener una lista de asteroides potencialmente 
peligrosos, y se devuelve una lista con los tres asteroides más grandes.

##### Ejemplo
URL: `https://localhost:7296/asteroids?days=7`

Salida:
```
[
    {
        "nombre": "467460 (2006 JF42)",
        "diametro": 0.659997996,
        "velocidad": "99331.0754164726",
        "fecha": "2023-05-11T00:00:00",
        "planeta": "Earth"
    },
    {
        "nombre": "(2019 CH1)",
        "diametro": 0.24521250665,
        "velocidad": "134486.710442838",
        "fecha": "2023-05-13T00:00:00",
        "planeta": "Earth"
    },
    {
        "nombre": "(2022 UD9)",
        "diametro": 0.18773386305,
        "velocidad": "23950.9670359407",
        "fecha": "2023-05-16T00:00:00",
        "planeta": "Earth"
    }
]
```

>*Los datos obtenidos varían según el día, pues la peticion se realiza a partir de la fecha actual*

<a name="casos-de-prueba"></a>
### Casos de prueba 💊
|Entrada|Salida|
| ----- | ---- |
| Rango entre 1 y 7 | Respuesta correcta |
| Menor que 1 | BadRequest("Introduce un número entre 1 y 7.") |
| Mayor que 7 | BadRequest("Introduce un número entre 1 y 7.") |
| NULL | BadRequest("El valor del parámetro days no puede ser nulo.") |
| String | BadRequest("El valor del parámetro days debe ser un número.") |
