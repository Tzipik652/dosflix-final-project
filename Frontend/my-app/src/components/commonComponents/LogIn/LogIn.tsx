import React, { FC, useEffect } from "react";
import {
  Box,
  TextField,
  Button,
  Typography,
  FormControlLabel,
  Checkbox,
  Alert,
  CircularProgress,
  Divider,
} from "@mui/material";
import { useFormik } from "formik";
import * as Yup from "yup";
import axios from "axios";
import { useDispatch } from "react-redux";
import { loginUser } from "../../../redux/authSlice";
import { useNavigate, Link } from "react-router-dom";
import { useGoogleLogin } from "@react-oauth/google";

const LogIn: FC<{}> = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [error, setError] = React.useState("");
  const [loading, setLoading] = React.useState(false);

  const formik = useFormik({
    initialValues: {
      email: "",
      password: "",
      rememberMe: false,
    },
    validationSchema: Yup.object({
      email: Yup.string().email("אימייל לא תקין").required("שדה חובה"),
      password: Yup.string().required("שדה חובה"),
    }),
    onSubmit: async (values) => {
      setError("");
      setLoading(true);

      try {
        const response = await axios.post(
          "https://localhost:7229/DosFlix/Users/login",
          {
            email: values.email,
            password: values.password,
          }
        );
        const { token, username, role } = response.data;
        dispatch(loginUser({ token, role, username }));
        localStorage.setItem("token", token);

        if (values.rememberMe) {
          localStorage.setItem("savedEmail", values.email);
          localStorage.setItem("savedPassword", values.password);
          localStorage.setItem("rememberMe", "true");
        } else {
          localStorage.removeItem("savedEmail");
          localStorage.removeItem("savedPassword");
          localStorage.setItem("rememberMe", "false");
        }

        navigate("/");
      } catch (err: any) {
        setError("אימייל או סיסמה לא נכונים");
      } finally {
        setLoading(false);
      }
    },
  });

  useEffect(() => {
    const savedEmail = localStorage.getItem("savedEmail");
    const savedPassword = localStorage.getItem("savedPassword");
    const remember = localStorage.getItem("rememberMe") === "true";

    if (remember && savedEmail && savedPassword) {
      formik.setValues({
        email: savedEmail,
        password: savedPassword,
        rememberMe: remember,
      });
    }
  }, []);

  const loginWithGoogle = useGoogleLogin({
    onSuccess: async (tokenResponse) => {
      try {
        const response = await axios.post(
          "https://localhost:7229/DosFlix/auth/google-login",
          {
            accessToken: tokenResponse.access_token,
          }
        );
        const { token, username, role } = response.data;
        dispatch(loginUser({ token, role, username }));
        localStorage.setItem("token", token);
        navigate("/");
      } catch {
        setError("התחברות עם Google נכשלה");
      }
    },
    onError: () => setError("התחברות עם Google נכשלה"),
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
        fontFamily: "Segoe UI",
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
        התחברות לחשבון
      </Typography>

      {error && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}

      <TextField
        fullWidth
        label="אימייל"
        type="email"
        margin="normal"
        {...formik.getFieldProps("email")}
        error={formik.touched.email && !!formik.errors.email}
        helperText={formik.touched.email && formik.errors.email}
      />

      <TextField
        fullWidth
        label="סיסמה"
        type="password"
        margin="normal"
        {...formik.getFieldProps("password")}
        error={formik.touched.password && !!formik.errors.password}
        helperText={formik.touched.password && formik.errors.password}
      />

      <FormControlLabel
        control={
          <Checkbox
            checked={formik.values.rememberMe}
            onChange={(e) =>
              formik.setFieldValue("rememberMe", e.target.checked)
            }
            name="rememberMe"
          />
        }
        label="זכור אותי"
      />

      <Button
        type="submit"
        fullWidth
        variant="contained"
        sx={{
          bgcolor: "#1fbac0",
          "&:hover": { bgcolor: "#18a2a7" },
          fontWeight: "bold",
          mt: 2,
        }}
        disabled={loading}
      >
        {loading ? <CircularProgress size={24} color="inherit" /> : "התחברות"}
      </Button>

      <Divider sx={{ my: 3 }}>או המשך עם</Divider>

      <Button
        fullWidth
        variant="outlined"
        onClick={() => loginWithGoogle()}
        sx={{
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          gap: 1,
        }}
      >
        <img
          src="https://www.svgrepo.com/show/475656/google-color.svg"
          alt="Google"
          width="20"
          height="20"
        />
        Google
      </Button>

      <Typography textAlign="center" mt={3} fontSize={14}>
        עדיין לא רשום?{" "}
        <Link to="/signup" style={{ color: "#1fbac0", textDecoration: "none" }}>
          הרשם עכשיו
        </Link>
      </Typography>
    </Box>
  );
};
export default LogIn;
