# 📱 Device API — REST API for Device Management

This project is a simple API made to **create, update, fetch, list, and delete devices**.  
The goal is to showcase clean design, validation, and REST best practices.

---

## 🚀 Supported Features

The API allows you to:

- **Create** a new device
- **Update** an existing device
- **Delete** a device  
- **Fetch** a single device
- **Fetch all devices**
- **Filter devices** by brand
- **Filter devices** by state

---

# 🐳 Running the Project with Docker

This application is fully **containerized**, so all you need is:

- **Docker**
- **Docker Compose**

If the command `docker ps` works on your machine, you're good to go.

---

# ▶️ How to Run the API

1. Open your terminal and navigate to the folder containing `docker-compose.yml`.
2. Run:

```bash
docker-compose up --build
```

Once everything starts, the API will be available at:

```
https://localhost:8081/devices
```

Swagger documentation:

```
https://localhost:8081/swagger/index.html
```

---

# 🌐 API Routes & Usage Examples

Below are the main endpoints and example requests.

---

## 📍 **GET /devices/{id}**  
Fetch a device by its ID.

```
GET https://localhost:8081/devices/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

### Example Response

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "iPhone 14",
  "brand": "Apple",
  "state": "Available"
}
```

---

## 📍 **GET /devices**  
Fetch all devices or apply filters.

### ➤ Fetch all:
```
GET https://localhost:8081/devices
```

### ➤ Filter by name:
```
GET https://localhost:8081/devices?name=iphone
```

### ➤ Filter by brand:
```
GET https://localhost:8081/devices?brand=samsung
```

### ➤ Filter by name + brand:
```
GET https://localhost:8081/devices?name=galaxy&brand=samsung
```

---

## 📍 **POST /devices**  
Create a new device.

```
POST https://localhost:8081/devices
Content-Type: application/json
```

### Request Body

```json
{
  "device": {
    "name": "Galaxy S24",
    "brand": "Samsung",
    "state": "Available"
  }
}
```

---

## 📍 **PUT /devices**  
Fully update a device.

```
PUT https://localhost:8081/devices
Content-Type: application/json
```

### Request Body

```json
{
  "device": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "Galaxy S24 Ultra",
    "brand": "Samsung",
    "state": "InUse"
  }
}
```

---

## 📍 **DELETE /devices/{id}**  
Delete a device by ID.

```
DELETE https://localhost:8081/devices/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

---

# 🛑 Stopping the Application

To shut everything down:

```bash
docker-compose down
```

If you also want to remove volumes:

```bash
docker-compose down -v
```

---

# 🔄 Rebuilding After Code Changes

If you modify the code and need Docker to rebuild:

```bash
docker-compose up --build
```

---
