import { configureStore } from "@reduxjs/toolkit";
import { useDispatch, useSelector } from "react-redux";
import { groupSlice } from "./features/groupSlice";
import { isAdminSlice } from "./features/isAdminSlice";
import { themeSlice } from "./features/themeSlice";

export const store = configureStore({
    reducer: {
        currentGroup: groupSlice.reducer,
        isAdmin: isAdminSlice.reducer,
        theme: themeSlice.reducer,
    }
})

export const useAppDispatch = useDispatch;
export const useAppSelector = useSelector;