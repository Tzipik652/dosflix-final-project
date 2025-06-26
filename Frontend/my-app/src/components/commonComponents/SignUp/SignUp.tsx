import React, { FC } from "react";
import {
  Box,
  Button,
  TextField,
  Typography,
  MenuItem,
  FormControl,
  InputLabel,
  Select,
  Alert,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import { useFormik } from "formik";
import * as Yup from "yup";
import Camera from "../../userComponents/Camera/Camera";
import axios from "axios";

const SignUp: FC<{}> = () => {
  const navigate = useNavigate();
  const [error, setError] = React.useState("");

  const formik = useFormik({
    initialValues: {
      username: "",
      email: "",
      password: "",
      phone: "",
      gender: "male", // או "female"
      ageGroup: "",
    },
    validationSchema: Yup.object({
      username: Yup.string().required("שדה חובה"),
      email: Yup.string().email("אימייל לא תקין").required("שדה חובה"),
      password: Yup.string().min(6, "לפחות 6 תווים").required("שדה חובה"),
      phone: Yup.string().required("שדה חובה"),
      gender: Yup.string().required("שדה חובה"),
      ageGroup: Yup.string().required("שדה חובה"),
    }),
    onSubmit: async (values: {
      username: any;
      email: any;
      password: any;
      phone: any;
      gender: string;
      ageGroup: string | number;
    }) => {
      setError("");
      try {
        await axios.post("https://localhost:7229/DosFlix/Users/register", {
        username: values.username,
        email: values.email,
        password: values.password,
        phone: values.phone,
        gender: values.gender === "male",
        ageGroup: +values.ageGroup,
        profilePicture: null,
      });

      localStorage.setItem("savedEmail", values.email);
      localStorage.setItem("savedPassword", values.password);
      localStorage.setItem("rememberMe", "true");

      navigate("/login");

      } catch (err: any) {
        setError(err.response?.data || "ההרשמה נכשלה");
      }
    },
  });

  return (
    <Box
      component="form"
      onSubmit={formik.handleSubmit}
      sx={{
        direction: "rtl",
        maxWidth: 400,
        mx: "auto",
        mt: 5,
        p: 3,
        boxShadow: 3,
        borderRadius: 2,
        bgcolor: "#fff",
      }}
    >
       <Box display="flex" justifyContent="center" mb={2}>
              <img
                src="https://ayeletginzburg.com/wp-content/uploads/2024/05/אייקון-פלאי-גדול-חלול-1024x1024.png"
                alt="Logo"
                style={{ width: 80, height: 80 }}
              />
            </Box>
      
      <Typography variant="h5" textAlign="center" color="#107d88" mb={2}>
        הרשמה לחשבון חדש
      </Typography>

      {error && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}

      <TextField
        fullWidth
        label="שם משתמש"
        margin="normal"
        {...formik.getFieldProps("username")}
        error={formik.touched.username && !!formik.errors.username}
        helperText={
          formik.touched.username && typeof formik.errors.username === "string"
            ? formik.errors.username
            : ""
        }
      />

      <TextField
        fullWidth
        label="אימייל"
        margin="normal"
        type="email"
        {...formik.getFieldProps("email")}
        error={formik.touched.email && !!formik.errors.email}
        helperText={
            formik.touched.email && typeof formik.errors.email === "string"
              ? formik.errors.email
              : ""
          }
      />

      <TextField
        fullWidth
        label="סיסמה"
        margin="normal"
        type="password"
        {...formik.getFieldProps("password")}
        error={formik.touched.password && !!formik.errors.password}
        helperText={
          formik.touched.password && typeof formik.errors.password === "string"
            ? formik.errors.password
            : ""
        }
      />

      <TextField
        fullWidth
        label="טלפון"
        margin="normal"
        type="tel"
        {...formik.getFieldProps("phone")}
        error={formik.touched.phone && !!formik.errors.phone}
        helperText={
          formik.touched.phone && typeof formik.errors.phone === "string"
            ? formik.errors.phone
            : ""
        }
      />
      <FormControl fullWidth margin="normal">
        <InputLabel id="ageGroup-label">קבוצת גיל</InputLabel>
        <Select
          labelId="ageGroup-label"
          label="קבוצת גיל"
          {...formik.getFieldProps("ageGroup")}
        >
          <MenuItem value="1">מתחת ל־18</MenuItem>
          <MenuItem value="2">18–30</MenuItem>
          <MenuItem value="3">31–50</MenuItem>
          <MenuItem value="4">51 ומעלה</MenuItem>
        </Select>
      </FormControl>



      <Box mt={2} mb={2}>
        <Camera
          onGenderDetected={(detectedGender) => {
            formik.setFieldValue("gender", detectedGender);
          }}
        />
      </Box>

      <Button
        fullWidth
        type="submit"
        variant="contained"
        sx={{
          bgcolor: "#1fbac0",
          "&:hover": { bgcolor: "#18a2a7" },
          fontWeight: "bold",
        }}
      >
        הרשמה
      </Button>
    </Box>
  );
};
export default SignUp;
