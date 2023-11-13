import { configureStore } from "@reduxjs/toolkit";
import { useDispatch, useSelector } from "react-redux";
import { groupSlice } from "./features/groupSlice";
import { isAdminSlice } from "./features/isAdminSlice";

export const store = configureStore({
    reducer: {
        currentGroup: groupSlice.reducer,
        isAdmin: isAdminSlice.reducer,
    }
})

export const useAppDispatch = useDispatch;
export const useAppSelector = useSelector;