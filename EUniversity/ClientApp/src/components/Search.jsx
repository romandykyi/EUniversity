import { React, useState } from 'react';

const Search = ({
    setInputValue,
    inputValue,
    setFoundItems,
    link
}) => {

    const [isResponsePossible, setIsResponsePossible] = useState(true);
    const [timeoutId, setTimeoutId] = useState(null);

    const search= async e => {
        setInputValue(e.target.value);
    
        if (e.target.value.length && isResponsePossible) {
            if (timeoutId) {
                clearTimeout(timeoutId);
            }
    
            const newTimeoutId = setTimeout(async () => {
                setIsResponsePossible(false);
                try {
                    const response = await fetch(`${link}${inputValue}`);
                    
                    if (response.ok) {
                        const data = await response.json();
                        setFoundItems(data.items);
                    } else {
                        console.log('error');
                    }
                } catch (error) {
                    console.log(error);
                } finally {
                    setIsResponsePossible(true);
                }
            }, 500);
    
            setTimeoutId(newTimeoutId);
        } else {
            setFoundItems([]);
            setIsResponsePossible(true);
        }
    };

    return (
        <input 
            className="form-control max-w-[250px]" 
            type="text" 
            placeholder="search . . ." 
            onChange={search}
        />
    );
};

export default Search;