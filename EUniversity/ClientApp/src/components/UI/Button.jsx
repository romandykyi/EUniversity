

const Button = ({ children, addStyles, ...props }) => {
    return (
        <button className={`px-3 py-2 font-medium text-white text-xl bg-theme rounded-lg outline-none border-none transition-transform duration-200 cursor-pointer hover:bg-blue-600 active:transform-active disabled:bg-gray-500 disabled:cursor-not-allowed disabled:active:transform-none ${addStyles}`} {...props}>
            {children}
        </button>
    )
}

export default Button;