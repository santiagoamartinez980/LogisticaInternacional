# Logística Internacional — aplicación de estudio local

Aplicación educativa en español para el semestre de Teoría de Materia de Operación Logística Internacional. Stack: ASP.NET Core Razor Pages + Minimal APIs, JavaScript nativo y CSS puro. No requiere autenticación, CDN, servicios externos, base de datos instalada ni paquetes NuGet adicionales.

## Requisitos

- .NET 8 SDK (o una versión compatible con el `TargetFramework` del proyecto).
- Navegador moderno.

## Ejecutar

```bash
cd LogisticaInternacional
dotnet restore
dotnet run
```

Abre en el navegador la URL local que indique la consola (normalmente `http://localhost:5xxx` o `https://localhost:7xxx`). Para fijar puerto:

```bash
dotnet run --urls http://localhost:5080
```

## Estructura

```text
LogisticaInternacional/
├── Data/
│   ├── theory.json       # 50 subapartados, teoría, ejemplo y error frecuente
│   ├── questions.json   # banco único inicial: 150 preguntas
│   ├── glossary.json    # 80+ términos
│   ├── cases.json       # 10 casos aplicados
│   └── resources.json
├── Pages/
│   ├── Index.cshtml     # shell Razor Pages
│   └── _ViewImports.cshtml
├── Services/
│   ├── ContentRepository.cs
│   └── QuizEngine.cs    # único servicio de selección de preguntas
├── wwwroot/
│   ├── app.js           # navegación y comportamiento sin frameworks
│   └── styles.css
├── Program.cs           # Minimal APIs
└── LogisticaInternacional.csproj
```

## Regla de cuestionario único

- El banco completo se carga desde `Data/questions.json`.
- `QuizEngine` es el servicio central para filtrar por tema, subtema, dificultad, cantidad y aleatoriedad.
- Dashboard, teoría, repaso y simulacro llaman a `startQuiz(...)`, la misma función del cliente, con filtros diferentes.
- La sesión actual y respuestas permanecen en memoria al cambiar de módulo; el menú principal siempre está visible y hay accesos a teoría/dashboard dentro del cuestionario.
- No hay prerrequisitos, candados, límites de intentos ni nota mínima.
- Los errores se registran en `localStorage` para estadísticas y repaso opcional.

## Almacenamiento y privacidad

Las notas, el modo visual, respuestas/estadísticas y errores se guardan en `localStorage` en el navegador. No se solicitan datos personales. El contenido de teoría y preguntas se lee de JSON local al iniciar el servidor. La versión inicial no usa SQLite porque JSON más `localStorage` evita configuración y mantiene el proyecto ligero. No almacenes información sensible en las notas si el dispositivo es compartido.

## API local

- `GET /api/topics`
- `GET /api/theory?topic=2&q=proveedor`
- `GET /api/questions?topic=2&difficulty=Intermedia&count=20&random=true`
- `GET /api/glossary?q=aduana`
- `GET /api/cases`
- `GET /api/resources`
- `GET /api/stats`

## Ampliar el contenido

### Añadir teoría
Edita `Data/theory.json`. Cada elemento usa `id`, `topic`, `topicTitle`, `title`, `definition`, `explanation`, `example`, `commonError`, `reviewQuestion` y `keyConcepts`.

### Añadir preguntas
Añade registros a `Data/questions.json`. IDs únicos recomendados. `answerIndex` es índice base cero en `options`. `type` puede ser `multiple` o `truefalse` en el banco inicial. La interfaz ya ofrece retroalimentación automática. Las preguntas abiertas y emparejamiento pueden agregarse extendiendo el modelo y la UI sin crear otro motor.

### Cambiar criterios del cuestionario
Mantén la selección de banco en `Services/QuizEngine.cs` y la sesión en `startQuiz` de `wwwroot/app.js`. No dupliques bancos por módulo. Para que el repaso de errores funcione con nuevas preguntas, conserva IDs estables.

## Contenido incluido

- 10 temas y 50 subapartados.
- 150 preguntas compartidas (tres por subapartado).
- Más de 80 términos de glosario.
- 10 casos prácticos, uno por tema.
- Calculadora de costo total de compra, comparador ponderado de proveedores, matriz de Kraljic arrastrable e indicadores OTIF/costo logístico.
- Notas locales exportables como TXT; modo claro/oscuro.
- Responsive y sin dependencias de frontend.

## Alcance académico

El material es una base de estudio general, no sustituye las lecturas, diapositivas ni criterios específicos de la asignatura. Verifica fórmulas, normativa aduanera, tributos e Incoterms con fuentes vigentes y con el docente. Los enlaces externos son opcionales; el contenido local funciona sin ellos.
