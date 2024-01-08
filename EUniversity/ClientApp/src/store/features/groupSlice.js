import { createSlice } from '@reduxjs/toolkit';

const initialState = {
  currentGroup: {
    id: 0,
    name: '',
  },
};

export const groupSlice = createSlice({
  name: 'CurrentGroup',
  initialState,
  reducers: {
    changeCurrentGroup: (state, action) => {
      state.currentGroup = action.payload;
    },
  },
});

export default groupSlice.reducer;
export const { changeCurrentGroup } = groupSlice.actions;
