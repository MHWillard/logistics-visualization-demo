   // Fetches from your .NET backend at e.g. http://localhost:5088/api/data/monthly
   export async function getMonthlyOrderStats() {
    const API_BASE = process.env.NEXT_PUBLIC_API_BASE_URL;

    const response = await fetch(`${API_BASE}/api/data/monthly`);
       if (!response.ok) {
         console.error('Failed to fetch monthly order stats');
         throw new Error('Failed to fetch monthly order stats');
       }
       console.log('Monthly order stats fetched successfully');
       const data = await response.json();
       console.log('Monthly order stats data:', data);
       return data;
     }

   export async function getMonthlyIncomeSummary() {
    const API_BASE = process.env.NEXT_PUBLIC_API_BASE_URL;

    const response = await fetch(`${API_BASE}/api/data/monthly-income-summary`);
       if (!response.ok) {
         console.error('Failed to fetch monthly income summary');
         throw new Error('Failed to fetch monthly income summary');
       }
       console.log('Monthly income summary fetched successfully');
       const data = await response.json();
       console.log('Monthly income summary data:', data);
       return data;
     }
