import { useState } from "react"
import Button from "../UI/Button/Button";

import {useNavigate} from "react-router-dom";

// TT DO:
// do a show password button

const ChangePassword = () => {

    const [formData, setFormData] = useState({
        currentPassword: '',
        newPassword: '',
    });
    const [error,setError] = useState('');
    const [isPasswordCahnged, setIsPasswordCahnged] = useState(false);
    const [checkBoxes, setCheckBoxes] = useState({
       checkbox1: false,
       checkbox2: false,
    });
    const navigate = useNavigate();

    const handleInputChange = e => {

        const { name, value } = e.target;

       if (name === "newPassword") {
           const hasUpperCase = /[A-Z]/.test(value);
           const hasNumber = /\d/.test(value);
           const hasDistinctSymbols = new Set(value).size >= 3;
           const isLengthValid = value.length >= 8;

           if (isLengthValid && hasUpperCase && hasNumber && hasDistinctSymbols) {
               setError(false);
           }
           else {
               let errorMessage = 'New password must meet the following conditions:';
               if (!isLengthValid) errorMessage += 'Be at least 8 characters long,\n';
               if (!hasUpperCase) errorMessage += 'Contain at least one uppercase letter,\n';
               if (!hasNumber) errorMessage += 'Contain at least one digit,\n';
               if (!hasDistinctSymbols) errorMessage += 'Contain at least 3 distinct symbols\n';

               setError(errorMessage.trim());
           }
       };
        setFormData({
            ...formData,
            [name]: value,
        });
    };
    const handleCheckboxChanges = e => {
        const {name, checked} = e.target;
        setCheckBoxes({
            ...checkBoxes,
            [name]: checked,
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const { currentPassword, newPassword } = formData;
            const requestBody = {
                current: currentPassword,
                new: newPassword,
            };

            try {
                const response = await fetch("/api/auth/password/change", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(requestBody),
                });

                if (response.ok) {
                    setIsPasswordCahnged(true);
                    navigate('/');
                    setIsPasswordCahnged(false);
                } else {
                    switch (response.status) {
                        case 401:
                            setError('Current password is not correct');
                            break;
                        default:
                            setError('An error occurred while changing the password.');
                    }
                }
            } catch (error) {
                console.error("An error occurred:", error);
                setError('An error occurred while changing the password.');
            }
        };


    return (
        <div className="changePassword form">
            <div className="changePassword__title form__title">
                Change your password
            </div>
            <form onSubmit={handleSubmit} className="changePassword form__form">
                <div className="changePassword__inputs form__inputs">
                    <input
                        type={checkBoxes.checkbox1 ? "text" : "password"}
                        value={formData.currentPassword}
                        className="changePassword__input form__input form-control"
                        placeholder="current password"
                        onChange={handleInputChange}
                        name="currentPassword"
                    />
                    <div className="changePassword__checkboxDiv">
                        <input
                            type="checkbox"
                            checked={checkBoxes.checkbox1}
                            className="changePassword__checkbox form__checkbox form-check-input"
                            onChange={handleCheckboxChanges}
                            name="checkbox1"
                            id="showPassword1"
                        />
                        <label className="form-check-label" htmlFor="showPassword1">show password</label>
                    </div>
                    <input
                        type={checkBoxes.checkbox2 ? "text" : "password"}
                        value={formData.newPassword}
                        className="changePassword__input form__input form-control"
                        placeholder="new password"
                        onChange={handleInputChange}
                        name="newPassword"
                    />
                    <div className="changePassword__checkboxDiv">
                        <input
                            type="checkbox"
                            checked={checkBoxes.checkbox2}
                            className="changePassword__checkbox form__checkbox form-check-input"
                            onChange={handleCheckboxChanges}
                            name="checkbox2"
                            id="showPassword2"
                        />
                        <label className="form-check-label" htmlFor="showPassword2">show password</label>
                    </div>
                    <div className="changePassword__error form__error">
                        {error}
                    </div>

                </div>
                <Button type="submit">Change password</Button>

            </form>
        </div>
    )
};

export default ChangePassword;