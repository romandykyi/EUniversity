import { createSlice } from "@reduxjs/toolkit";


const initialState = {
    isAdmin: false
};

export const isAdminSlice = createSlice({
    name: 'isAdmin',
    initialState,
    reducers: {
        setIsAdmin: (state, action) => {
            state.isAdmin = action.payload;
        }
    }
})

export default isAdminSlice.reducer;
export const { setIsAdmin } = isAdminSlice.actions;