import React, { useEffect, useRef } from 'react';

const Test = () => {

    const inputRef = useRef();

    useEffect(() => {
        inputRef.current.value = "amogus";
    }, []);

    return (
        <div>
            <input
                type="text" 
                ref={inputRef}
            />
        </div>
    );
};

export default Test;