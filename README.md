# 🎬 Dosflix – Final Project

Dosflix is a full-stack movie platform that allows users to explore, order, and securely access films.  
The system combines a **React frontend**, a **C# ASP.NET Core backend**, and integrated **AI modules** for smart classification and user interaction.

---

## 📌 Overview

This project includes:

- **Frontend (React + MUI):**
  - User registration and login
  - Movie browsing and filtering
  - Movie ordering with secure access credentials
  - Responsive and modern UI built with **Material UI (MUI)** components

- **Backend (C# ASP.NET Core):**
  - RESTful API for user, movie, and order management
  - Authentication and authorization with JWT
  - Email notifications to users after order processing

- **AI Modules (Python):**
  - Gender and age detection
  - Logo recognition
  - Movie categorization

---

## 🛠 Tech Stack

### Frontend
- React + TypeScript
- Material UI (MUI)
- Axios
- React Router
- Formik / Yup (for form handling and validation)

### Backend
- C# (.NET 7)
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT for secure authentication

### AI & ML
- Python (Flask)
- TensorFlow / PyTorch
- OpenCV for image processing

---

## 🚀 Getting Started
### Frontend
```bash
cd client
npm install
npm start
```
### AI
```bash
cd ai
pip install -r requirements.txt
python app.py
