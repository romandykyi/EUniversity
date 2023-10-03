
const defaultState = {
    isPasswordChanged: false,
};

const CHANGE_STATE = "CHANGE_STATE"

export const isPasswordChangedReducer = (state = defaultState, action) => {
    switch (action.type) {
        case CHANGE_STATE:
            return {...state, isPasswordChanged: action.payload};
        default:
            return state;
    }
};

export const changeIsPasswordChangedAction = (payload) => ({type:CHANGE_STATE, payload});

