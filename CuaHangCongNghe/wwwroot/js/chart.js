const url = '/Admin/statistical'; 

fetch(url)
    .then(response => response.json())
    .then(data => {
        // Dữ liệu JSON từ action statistical nằm trong biến data
        const statisticalData = data;

        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);
        function drawChart() {
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Day');
            data.addColumn('number', 'Doanh thu')                  

            const dataRows = Object.entries(statisticalData.sta).map(([date, revenue]) => {
                return [date, revenue];
            });
            data.addRows(dataRows);

            var options = {
                title: 'Company Performance',
                hAxis: { title: 'Day', titleTextStyle: { color: '#333' } },
                vAxis: { minValue: 0 }
            };

            var chart = new google.visualization.AreaChart(document.getElementById('chart_div'));
            chart.draw(data, options);
        }

    })
    .catch(error => console.log(error));
    
