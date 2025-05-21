import * as React from 'react';
import '../App.css'

import {
    DataGrid,
    GridActionsCellItem,
} from '@mui/x-data-grid';
import {
    Button,
    TextField,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
} from '@mui/material';
import { Delete, Add } from '@mui/icons-material';

export default function CrudDataGridWithAPI() {
    const [rows, setRows] = React.useState([]);
    const [loading, setLoading] = React.useState(true);
    const [newRow, setNewRow] = React.useState({ id: '', date: '' });
    const [open, setOpen] = React.useState(false);
    const [pageSize, setPageSize] = React.useState(5);

    const API_URL = "https://localhost:7184/api/Weather";

    // 🔄 Fetch data on mount
    React.useEffect(() => {
        const fetchData = async () => {
            try {
                const res = await fetch(API_URL);
                const data = await res.json();
                setRows(data);
            } catch (err) {
                console.error('Fetch error:', err);
            } finally {
                setLoading(false);
            }
        };
        fetchData();
    }, []);

    // ✏️ Update
    const processRowUpdate = async (updatedRow: { id: any; }) => {
        try {
            const res = await fetch(`${API_URL}/${updatedRow.id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(updatedRow),
            });
            if (!res.ok) throw new Error('Update failed');
            const newData = await res.json();
            setRows((prev) =>
                prev.map((row) => (row.id === updatedRow.id ? newData : row))
            );
            return newData;
        } catch (err) {
            console.error('Update error:', err);
            return updatedRow;
        }
    };

    // ❌ Delete
    const handleDelete = async (id: unknown) => {
        try {
            const res = await fetch(`${API_URL}/${id}`, { method: 'DELETE' });
            if (!res.ok) throw new Error('Delete failed');
            setRows((prev) => prev.filter((row) => row.id !== id));
        } catch (err) {
            console.error('Delete error:', err);
        }
    };

    // ➕ Add
    const handleAddNew = async () => {
        try {
            const newUser = { id: Number(newRow.id), date: newRow.date };
            const res = await fetch(API_URL, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(newUser),
            });
            if (!res.ok) throw new Error('Add failed');
            const created = await res.json();
            setRows((prev) => [...prev, created]);
            setNewRow({ id: '', date: '' });
            setOpen(false);
        } catch (err) {
            console.error('Add error:', err);
        }
    };

    const columns = [
        { field: 'id', headerName: 'ID', width: 90, editable: false },
        { field: 'date', headerName: 'Date', width: 150, editable: true },
        {
            field: 'actions',
            headerName: 'Actions',
            type: 'actions',
            getActions: (params: { id: unknown; }) => [
                <GridActionsCellItem
                    icon={<Delete />}
                    label="Delete"
                    onClick={() => handleDelete(params.id)}
                />,
            ],
        },
    ];

    return (
        <div style={{ height: 400, width: '100%' }}>
            <Button
                startIcon={<Add />}
                onClick={() => setOpen(true)}
                style={{ marginBottom: 10 }}
                variant="contained"
            >
                Add New
            </Button>

            <DataGrid
                rows={rows}
                columns={columns}
                loading={loading}
                pageSize={pageSize}
                onPageSizeChange={(newSize: React.SetStateAction<number>) => setPageSize(newSize)}
                rowsPerPageOptions={[5, 10]}
                processRowUpdate={processRowUpdate}
                pagination
                experimentalFeatures={{ newEditingApi: true }}
            />

            {/* Add New Row Dialog */}
            <Dialog open={open} onClose={() => setOpen(false)}>
                <DialogTitle>Add New Row</DialogTitle>
                <DialogContent>
                    <TextField
                        margin="dense"
                        label="ID"
                        type="number"
                        fullWidth
                        value={newRow.id}
                        onChange={(e) => setNewRow({ ...newRow, id: e.target.value })}
                    />
                    <TextField
                        margin="dense"
                        label="Date"
                        fullWidth
                        value={newRow.date}
                        onChange={(e) => setNewRow({ ...newRow, date: e.target.value })}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setOpen(false)}>Cancel</Button>
                    <Button onClick={handleAddNew}>Add</Button>
                </DialogActions>
            </Dialog>
        </div>
    );
}
