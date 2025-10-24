# Prueba Técnica – Carsales  
**Autor:** Mario Chiang  
**Rol:** Ingeniero de Software  

---

## Descripción general

Este proyecto corresponde a la **prueba técnica para el cargo de Software Engineer Semi Senior** en Carsales.  
El objetivo es consumir la API pública de **Rick and Morty** mediante un backend intermedio (BFF .NET 8) y un frontend desarrollado en **Angular**, aplicando buenas prácticas de arquitectura, manejo de errores y separación de responsabilidades.

La aplicación permite listar los episodios de la serie, buscar por nombre y navegar entre páginas de resultados.

---

## Tecnologías utilizadas

### Backend
- **.NET 8 (Web API)**
- **Polly** (reintentos y resiliencia)
- **HttpClientFactory**
- **Inyección de dependencias**
- **Swagger** para documentación

### Frontend
- **Angular 20**
- **Standalone Components**
- **Signals**
- **TypeScript**
- Sin frameworks CSS (solo estilos básicos)

---

## Arquitectura

El proyecto está dividido en dos capas principales:

```
Carsales.Bff.Api/      → Backend (BFF .NET 8)
frontend/              → Frontend Angular
```

El **BFF** actúa como intermediario entre el frontend y la API pública  
`https://rickandmortyapi.com/api/`, cumpliendo el patrón *Backend for Frontend*.

---

## Entorno de desarrollo

El repositorio contiene **dos partes principales**, que deben ejecutarse en entornos distintos:

| Componente | Carpeta | Entorno recomendado | Descripción |
|-------------|----------|--------------------|--------------|
| **Backend (BFF)** | `Carsales.Bff.Api` | Visual Studio 2022 | Proyecto .NET 8 (API REST). Se ejecuta con `dotnet run` o desde Visual Studio. |
| **Frontend (Angular)** | `frontend` | Visual Studio Code | Aplicación Angular 20, ejecutable con `npm start` o `ng serve --open`. |

Esto permite mantener la separación clara entre la lógica del backend (API) y la vista del frontend (Angular), siguiendo el patrón **BFF – Backend for Frontend**.

---

## Ejecución del proyecto

### 1. Requisitos previos
- Tener instalado **.NET 8 SDK**
- Tener instalado **Node.js 20+**
- Tener **Angular CLI** (se instala con `npm install -g @angular/cli`)

---

### 2. Ejecutar el backend
```bash
cd Carsales.Bff.Api
dotnet restore
dotnet run
```

Por defecto se levanta en:
> **https://localhost:7234/swagger**

Allí se puede probar directamente el endpoint:
```
GET /api/Episodes?page=1
GET /api/Episodes/{id}
```

---

### 3. Ejecutar el frontend
En otra terminal:
```bash
cd frontend
npm install
npm start
```

La aplicación se ejecutará en:
> **http://localhost:4200**

A través de esta interfaz se pueden listar, buscar y paginar los episodios obtenidos desde el BFF.

---

## Configuración

En `Carsales.Bff.Api/appsettings.json`:

```json
"RickAndMorty": {
  "BaseUrl": "https://rickandmortyapi.com/api/",
  "RetryCount": 3
}
```

El `HttpClient` usa esta URL base para comunicarse con la API pública, aplicando reintentos con **Polly** y control de errores con `EnsureSuccessStatusCode`.

---

## Criterios de la prueba

| Requisito | Estado | Descripción |
|------------|---------|-------------|
| Paginación | ✅ | Implementada en backend y frontend |
| Manejo de errores | ✅ | Middleware y control de respuestas HTTP |
| Uso de SOLID / patrones | ✅ | Separación Controller / Service / Model |
| Archivos de configuración | ✅ | appsettings.json |
| Sin framework CSS | ✅ | Solo estilos propios |
| .NET 8 | ✅ | API desarrollada en .NET 8 |
| Angular moderno | ✅ | Standalone Components + Signals |
| Documentación | ✅ | README con pasos y configuración |
| Código en GitHub | ✅ | Repositorio público listo para revisión |

---

## Consideraciones técnicas

- El backend implementa **retry exponencial con jitter** para fallos 5xx o timeout.  
- El frontend se comunica solo con el BFF, nunca directamente con la API externa.  
- Todo el flujo cumple con el patrón **BFF (Backend for Frontend)**.  
- Se validan estados de carga, error y paginación.  
- Proyecto organizado para fácil mantenimiento y pruebas futuras.

---

## Ejemplo de flujo

1. Angular llama a:  
   `https://localhost:7234/api/Episodes?page=1`
2. El BFF traduce esa solicitud a:  
   `https://rickandmortyapi.com/api/episode?page=1`
3. Devuelve una respuesta normalizada y paginada al frontend.

---

## Cómo probar
1. Clonar el repositorio desde GitHub.  
2. Seguir los pasos de ejecución.  
3. Confirmar acceso al listado de episodios y probar la búsqueda.  
4. Revisar Swagger si se desea validar la API directamente.

---