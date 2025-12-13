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

# 🐳 Running the Project with Visual Studio (Docker Support Enabled)

This application is fully **containerized**, and the easiest way to run it is directly through **Visual Studio**, which will take care of Docker for you.

## ▶️ How to Run Using Visual Studio

1. Open the solution (`.sln`) in **Visual Studio**.
2. Make sure Docker Desktop is running.
3. At the top toolbar, select the run option that looks like:

```
Docker Compose
```

4. Press **F5** or click **Run**.

Visual Studio will:

- Build the Docker images  
- Start the required containers  
- Launch the API automatically  

Once everything is up, the API will be available at:

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

To stop everything, simply **stop debugging** in Visual Studio (Shift + F5).  
Visual Studio will automatically shut down the containers it started.

---

# 🔄 Rebuilding After Code Changes

Visual Studio will rebuild automatically when you press **F5**, but if Docker caching causes issues:

1. Right-click the project  
2. Select **Clean**  
3. Then select **Rebuild**  
4. Run again with **Docker** selected  

---

