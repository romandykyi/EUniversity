import { configureStore } from "@reduxjs/toolkit";
import { useDispatch, useSelector } from "react-redux";
import { groupSlice } from "./features/groupSlice";

export const store = configureStore({
    reducer: {
        currentGroup: groupSlice.reducer,
    }
})

export const useAppDispatch = useDispatch;
export const useAppSelector = useSelector;