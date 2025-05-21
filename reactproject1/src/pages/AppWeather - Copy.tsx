import { useEffect, useState } from 'react'
//import reactLogo from './assets/react.svg'
//import viteLogo from '/vite.svg'
import './App.css'


type Author = {
    id: number;
    date: string;
    temperatureC: string;
};

const API_URL = "https://localhost:7184/api/Weather";


const fetchUsers = async (apiUrl: string): Promise<Author[]> => {
    const response = await fetch(apiUrl);

    if (!response.ok) {
        throw new Error(`Error: ${response.status} ${response.statusText}`);
    }

    return response.json();
};



function AppBlogger() {
    const [users, setUsers] = useState<Author[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const getUsers = async () => {
            try {
                const data = await fetchUsers(API_URL); // Call the function with the URL
                setUsers(data);
            } catch (err: unknown) {
                if (err instanceof Error) {
                    setError(err.message);
                } else {
                    setError('An unknown error occurred');
                }
            } finally {
                setLoading(false);
            }
        };

        getUsers();
    }, []); // Dependency array

    if (loading) return <p>Loading...</p>;
    if (error) return <p>Error: {error}</p>;

    return (
        <ul>
            {users.map((user) => (
                <li key={user.id}>
                    <strong>{user.date}</strong> - {user.temperatureC}
                </li>
            ))}
        </ul>
    );
};

export default AppBlogger;
