
import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { RootState, AppDispatch } from '../myStore'; 
import { clearCart } from './cartSlice';

type Role = 1 | 2 | null;

interface AuthState {
  isLoggedIn: boolean;
  role: Role;
  username: string;
  token: string;
}

const initialState: AuthState = {
  isLoggedIn: false,
  role: null,
  username: "",
  token: "",
};

export const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    loginUser: (state, action: PayloadAction<{ role: Role; username: string; token: string }>) => {
      state.isLoggedIn = true;
      state.role = action.payload.role;
      state.username = action.payload.username;
      state.token = action.payload.token;
    },
    logout(state) {
      state.isLoggedIn = false;
      state.role = null;
      state.username = "";
      state.token = "";
    },
  },
});

export const { loginUser, logout } = authSlice.actions;

export const logoutAndClearCart = () => (dispatch: AppDispatch) => {
  dispatch(logout());
  dispatch(clearCart());
};

export default authSlice.reducer;

// סלקטור (אם את משתמשת בו)
export const selectUsername = (state: RootState) => state.auth.username;
