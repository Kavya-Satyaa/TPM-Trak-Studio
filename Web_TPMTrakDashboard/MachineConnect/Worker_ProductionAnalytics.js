function fetchData(data) {
    fetch('ProductionAnalytics.aspx/getRunTimeChartData', {
        method: 'POST',
        mode: 'cors',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        body: JSON.stringify({ plant: data.plant, machine: data.machine, shift: data.shift, date: data.date, isautorefresh: data.isautorefresh }),
    }).then(response => response.json()
    ).then(data => {
        postMessage(data);
        //console.log("Data posted");
    });
}

function fetchTimeAnalysisData(data) {
    fetch('ProductionAnalytics.aspx/getTimeAnalysisChartData', {
        method: 'POST',
        mode: 'cors',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        body: JSON.stringify({ plant: data.plant, machine: data.machine, shift: data.shift, date: data.date, isautorefresh: data.isautorefresh }),
    }).then(response => response.json()
    ).then(data => {
        postMessage(data);
        //console.log("Data posted");
    });
}

function fetchStoppageData(data) {
    fetch('ProductionAnalytics.aspx/getStoppageReasonData', {
        method: 'POST',
        mode: 'cors',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        body: JSON.stringify({ plant: data.plant, machine: data.machine, shift: data.shift, date: data.date, isautorefresh: data.isautorefresh }),
    }).then(response => response.json()
    ).then(data => {
        postMessage(data);
        //console.log("Data posted");
    });
}


function fetchHourlyPartCountData(data) {
    fetch('ProductionAnalytics.aspx/getHourlyPartCountChartData', {
        method: 'POST',
        mode: 'cors',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        body: JSON.stringify({ plant: data.plant, machine: data.machine, shift: data.shift, date: data.date, shiftforhour: data.shiftforhour, isautorefresh: data.isautorefresh }),
    }).then(response => response.json()
    ).then(data => {
        postMessage(data);
        //console.log("Data posted");
    });
}

function fetchHourlyRunTimeData(data) {
    fetch('ProductionAnalytics.aspx/getHourlyRunTimeChartData', {
        method: 'POST',
        mode: 'cors',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        body: JSON.stringify({ plant: data.plant, machine: data.machine, shift: data.shift, date: data.date, isautorefresh: data.isautorefresh }),
    }).then(response => response.json()
    ).then(data => {
        postMessage(data);
        //console.log("Data posted");
    });
}


onmessage = function (e) {
    if (e.data.param == "RunTimeChart") {
        fetchData(e.data);
    } else if (e.data.param == "TimeAnalysis") {
        fetchTimeAnalysisData(e.data);
    } else if (e.data.param == "StoppageData") {
        fetchStoppageData(e.data);
    } else if (e.data.param == "HourlyPartCount") {
        fetchHourlyPartCountData(e.data);
    } else if (e.data.param == "HourlyRunTime") {
        fetchHourlyRunTimeData(e.data);
    }
}