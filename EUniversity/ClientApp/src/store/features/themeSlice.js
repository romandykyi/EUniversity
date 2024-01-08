import { createSlice } from '@reduxjs/toolkit';

const initialState = {
  isThemeDark: false,
};

export const themeSlice = createSlice({
  name: 'isThemeDark',
  initialState,
  reducers: {
    setIsThemeDarkRedux: (state, action) => {
      state.isThemeDark = action.payload;
    },
  },
});

export default themeSlice.reducer;
export const { setIsThemeDarkRedux } = themeSlice.actions;
