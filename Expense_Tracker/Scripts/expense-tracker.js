$(document).ready(function () {

    $('#table-member').DataTable({
        searching: false,
        paging: false,
        columnDefs: [
            { orderable: false, targets: -1 },
            { orderable: false, targets: -2 }
        ]
    });

    $('#table-category').DataTable({
        searching: false,
        paging: false,
        columnDefs: [
            { orderable: false, targets: -1 },
            { orderable: false, targets: -2 }
        ]
    });

    var table = $('#table-expense').DataTable({
        dom: 'Pfrtip',
        paging: false,
        columnDefs: [
            { orderable: false, targets: -1 },
            { orderable: false, targets: -2 }
        ]
    });

    var containerMember = document.getElementById("member-chart");
    var containerCategory = document.getElementById("category-chart");

    var chartMember = Highcharts.chart(containerMember, {
        chart: {
            type: 'pie'
        },
        title: {
            text: 'Expense Per Member'
        },
        series: [{
            name: 'Amount',
            data: memberData(table)
        }],
        tooltip: {
            valueDecimals: 2,
            valuePrefix: '$',
        },
        plotOptions: {
            pie: {
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: ${point.y:.1f} ({point.percentage:.1f} %)'
                }
            }
        },
        credits: {
            enabled: false
        }
    });

    table.on('draw', function () {
        chartMember.series[0].setData(memberData(table));
    });

    var chartCategory = Highcharts.chart(containerCategory, {
        chart: {
            type: 'pie'
        },
        title: {
            text: 'Expense Per Category'
        },
        series: [{
            name: 'Amount',
            data: categoryData(table)
        }],
        tooltip: {
            valueDecimals: 2,
            valuePrefix: '$',
        },
        plotOptions: {
            pie: {
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: ${point.y:.1f} ({point.percentage:.1f} %)'
                }
            }
        },
        credits: {
            enabled: false
        }
    });

    table.on('draw', function () {
        chartCategory.series[0].setData(categoryData(table));
    });
});

function memberData(table) {
    var amounts = {};

    table
        .column(3, { search: 'applied' })
        .data()
        .each(function (value, index) {
            if (amounts[value]) {
                amounts[value] += parseFloat(table.column(4).data()[index].substring(1));
            } else {
                amounts[value] = parseFloat(table.column(4).data()[index].substring(1));
            }
        });

    return $.map(amounts, function (val, key) {
        return {
            name: key,
            y: val
        };
    });
}

function categoryData(table) {
    var amounts = {};

    table
        .column(2, { search: 'applied' })
        .data()
        .each(function (value, index) {
            if (amounts[value]) {
                amounts[value] += parseFloat(table.column(4).data()[index].substring(1));
            } else {
                amounts[value] = parseFloat(table.column(4).data()[index].substring(1));
            }
        });

    return $.map(amounts, function (val, key) {
        return {
            name: key,
            y: val
        };
    });
}