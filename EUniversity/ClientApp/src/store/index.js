import {createStore, combineReducers, applyMiddleware} from "redux"
import {composeWithDevTools} from "redux-devtools-extension";
import thunk from "redux-thunk";
import {isPasswordChangedReducer} from "./isPasswordChangedReducer";


const rootReducer = combineReducers({
    isPasswordChanged:isPasswordChangedReducer,
});


export const store = createStore(rootReducer, composeWithDevTools(applyMiddleware(thunk)));

