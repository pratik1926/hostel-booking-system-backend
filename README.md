# Hostel Booking System (Backend)

A scalable and modular backend system for managing hostel bookings, built using **.NET**, following **Clean Architecture** and **SOLID principles**.

---

##  Features

*  User Authentication (Login, Register, Reset Password)
*  Hostel Management
*  Room Management & Booking System
*  Payment Integration (Initiate & Verify)
*  Feedback & Ratings System
*  Verification System (OTP / Code-based)
*  Admin Controls (Status Management)

---

##  Architecture

This project follows **Clean Architecture**:

* **Controllers** → Handle HTTP requests
* **DTOs** → Structured request/response handling
* **Services** → Business logic
* **Repositories** → Data access
* **Models** → Core entities

---

##  Project Structure

```
Application/
 └── DTOs/
      ├── User/
      ├── Booking/
      ├── Hostel/
      ├── Payment/
      ├── Verification/
      └── Common/
```

---

## ⚙️ Tech Stack

* .NET Core Web API
* Entity Framework Core
* SQL Server
* JWT Authentication
* REST API

---

##  Key Improvements (Recent Work)

* Refactored DTOs into domain-based structure
* Removed duplicate DTOs (LoginDTO)
* Improved naming conventions (Request/Response pattern)
* Applied SOLID principles in controllers

---

##  How to Run

1. Clone the repo

```bash
git clone https://github.com/pratik1926/hostel-booking-system-backend.git
```

2. Open in Visual Studio

3. Update database connection string

4. Run:

```bash
dotnet run
```

---

##  Future Enhancements

* Map integration for location-based booking
* Notification system
* Advanced filtering & search

---

##  Author

**Pratik Bohara**
