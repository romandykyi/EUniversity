import React, { useState, useEffect } from 'react';
import styles from "./PasswordChangedBlock.module.css";


const PasswordChangedBlock = () => {
    const [isVisible, setIsVisible] = useState(false);

    const showBanner = () => {
        setIsVisible(true);

        const timeout = setTimeout(() => {
            setIsVisible(false);
        }, 2000);

        return () => {
            clearTimeout(timeout);
        };
    };

    return (
        <div className={`${styles.banner} ${isVisible ? styles.visible : styles.hidden}`}>
            Your password was successfully changed!
        </div>
    );
};

export default PasswordChangedBlock;