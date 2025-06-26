// import React, { useRef, useState } from "react";
// import {
//   Box,
//   Button,
//   Typography,
//   CircularProgress,
//   Alert,
//   Stack,
// } from "@mui/material";

// interface CameraProps {
//   onGenderDetected: (gender: "male" | "female") => void;
// }

// const Camera: React.FC<CameraProps> = ({ onGenderDetected }) => {
//   const videoRef = useRef<HTMLVideoElement>(null);
//   const canvasRef = useRef<HTMLCanvasElement>(null);
//   const [gender, setGender] = useState<string | null>(null);
//   const [loading, setLoading] = useState(false);
//   const [error, setError] = useState<string | null>(null);
//   const [cameraPlay, setCameraPlay] = useState<boolean>(false);

//   const startCamera = async () => {
//     try {
//       const stream = await navigator.mediaDevices.getUserMedia({ video: true });
//       if (videoRef.current) {
//         videoRef.current.srcObject = stream;
//         setCameraPlay(true);
//       }
//     } catch {
//       setError("לא ניתן להפעיל את המצלמה");
//     }
//   };

//   const captureAndSend = async () => {
//     const video = videoRef.current;
//     const canvas = canvasRef.current;
//     if (!video || !canvas) return;

//     const ctx = canvas.getContext("2d");
//     if (!ctx) return;

//     canvas.width = video.videoWidth;
//     canvas.height = video.videoHeight;
//     ctx.drawImage(video, 0, 0, canvas.width, canvas.height);

//     setLoading(true);
//     setError(null);

//     canvas.toBlob(async (blob) => {
//       if (!blob) return;

//       const formData = new FormData();
//       formData.append("image", blob, "photo.jpg");

//       try {
//         const res = await fetch("http://localhost:5000/predict-gender", {
//           method: "POST",
//           body: formData,
//         });

//         if (!res.ok) throw new Error("Server error");

//         const result = await res.json();
//         setGender(result.gender);
//         onGenderDetected(result.gender);
//       } catch {
//         setError("לא ניתן לחבר לשרת. נסה שוב מאוחר יותר.");
//       } finally {
//         setLoading(false);
//       }
//     }, "image/jpeg");
//   };

//   return (
//     <Box mt={2}>
//       <Typography variant="subtitle1" color="#107d88" gutterBottom>
//         זיהוי מגדר אוטומטי
//       </Typography>

//       <Stack spacing={2} alignItems="start">
//         {!cameraPlay && (
//           <Button variant="outlined" onClick={startCamera}>
//             הפעל מצלמה
//           </Button>
//         )}

//         <video
//           ref={videoRef}
//           autoPlay
//           width="100%"
//           height="200"
//           style={{
//             border: "1px solid #a8e4e8",
//             borderRadius: "6px",
//             backgroundColor: "#f0fcfd",
//           }}
//         />

//         <canvas ref={canvasRef} style={{ display: "none" }} />

//         <Button
//           variant="contained"
//           onClick={captureAndSend}
//           disabled={loading || !cameraPlay}
//           sx={{ bgcolor: "#1fbac0", "&:hover": { bgcolor: "#18a2a7" } }}
//         >
//              {gender ? "לניסיון חוזר צלם שוב" : "צלם ושלח"}
//         </Button>

//         {loading && <CircularProgress size={24} color="primary" />}

//         {gender && (
//           <Alert severity="info">
//             המגדר שזוהה: {gender === "female" ? "נקבה" : "זכר"}
//           </Alert>
//         )}

//         {error && <Alert severity="error">{error}</Alert>}
//       </Stack>
//     </Box>
//   );
// };

// export default Camera;
import React, { useRef, useState } from "react";
import {
  Box,
  Button,
  Typography,
  CircularProgress,
  Alert,
  Stack,
} from "@mui/material";

interface CameraProps {
  onGenderDetected: (gender: "male" | "female") => void;
}

const Camera: React.FC<CameraProps> = ({ onGenderDetected }) => {
  const videoRef = useRef<HTMLVideoElement>(null);
  const canvasRef = useRef<HTMLCanvasElement>(null);
  const [gender, setGender] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [cameraPlay, setCameraPlay] = useState<boolean>(false);

  const startCamera = async () => {
    try {
      const stream = await navigator.mediaDevices.getUserMedia({ video: true });
      if (videoRef.current) {
        videoRef.current.srcObject = stream;
        setCameraPlay(true);
      }
    } catch {
      setError("לא ניתן להפעיל את המצלמה");
    }
  };

  const captureAndSend = async (blob: Blob) => {
    setLoading(true);
    setError(null);

    const formData = new FormData();
    formData.append("image", blob, "photo.jpg");

    try {
      const res = await fetch("http://localhost:5000/predict-gender", {
        method: "POST",
        body: formData,
      });

      if (!res.ok) throw new Error("Server error");

      const result = await res.json();
      setGender(result.gender);
      onGenderDetected(result.gender);
    } catch {
      setError("לא ניתן לחבר לשרת. נסה שוב מאוחר יותר.");
    } finally {
      setLoading(false);
    }
  };

  const handleFileUpload = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (!file) return;
    captureAndSend(file); // שלח את התמונה כמו שהיינו שולחים את התמונה מהמצלמה
  };

  const captureFromCamera = () => {
    const video = videoRef.current;
    const canvas = canvasRef.current;
    if (!video || !canvas) return;

    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    ctx.drawImage(video, 0, 0, canvas.width, canvas.height);

    canvas.toBlob((blob) => {
      if (blob) captureAndSend(blob);
    }, "image/jpeg");
  };

  return (
    <Box mt={2}>
      <Typography variant="subtitle1" color="#107d88" gutterBottom>
        זיהוי מגדר אוטומטי
      </Typography>

      <Stack spacing={2} alignItems="start">
        {!cameraPlay && (
          <Button variant="outlined" onClick={startCamera}>
            הפעל מצלמה
          </Button>
        )}

        <video
          ref={videoRef}
          autoPlay
          width="100%"
          height="200"
          style={{
            border: "1px solid #a8e4e8",
            borderRadius: "6px",
            backgroundColor: "#f0fcfd",
          }}
        />

        <canvas ref={canvasRef} style={{ display: "none" }} />

        <Button
          variant="contained"
          onClick={captureFromCamera}
          disabled={loading || !cameraPlay}
          sx={{ bgcolor: "#1fbac0", "&:hover": { bgcolor: "#18a2a7" } }}
        >
          {gender ? "לניסיון חוזר צלם שוב" : "צלם ושלח"}
        </Button>

        {/* ✨ חדש: כפתור בחירת תמונה מהמחשב */}
        <Button variant="outlined" component="label">
          העלי תמונה מהמחשב
          <input type="file" accept="image/*" hidden onChange={handleFileUpload} />
        </Button>

        {loading && <CircularProgress size={24} color="primary" />}
        {gender && (
          <Alert severity="info">
            המגדר שזוהה: {gender === "female" ? "נקבה" : "זכר"}
          </Alert>
        )}
        {error && <Alert severity="error">{error}</Alert>}
      </Stack>
    </Box>
  );
};

export default Camera;
