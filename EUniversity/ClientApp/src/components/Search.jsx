import { React, useState } from 'react';

const Search = ({
    setInputValue,
    search
}) => {

    const handleChange = e => {
        setInputValue(e.target.value);
    }

    return (
        <input 
            className="form-control max-w-[250px]" 
            type="text" 
            placeholder="search . . ." 
            onChange={e => {
                handleChange(e);
                search();
            }}
        />
    );
};

export default Search;