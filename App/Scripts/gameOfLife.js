var GameOfLife = {
    config: {
        canvas: '#canvas-test',
        intervalCounter: 0,
        intervals: 2,
        grid: {
            pixelSize: 10,
            lineWidth: 0.3,
            strokeColor: '#aaa'
        },
        cells : null
    },

    init: function() {
        this.getGridSettings();
        this.getDataFromServer();

    },

    getGridSettings: function () {
        var self = this;

        $.ajax(
            {
                type: 'POST',
                url: 'Home/GetGridSettings'
            }
        )
        .done(function (response, textStatus, jqXHR) {
            self.setupGrid(response);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {

            console.log('Failed : getGridSettings');
        })


    },

    getDataFromServer: function () {
        var self = this;

        $.ajax(
            {
                type: 'POST',
                url: 'Home/GetGenerationData',
                data: { cells : JSON.stringify(self.config.cells) }
            }
        )
        .done(function (response, textStatus, jqXHR) {

            self.config.cells = response;
            self.renderGrid();

            if (self.config.intervalCounter < self.config.intervals) {
                self.getDataFromServer();
                self.config.intervalCounter += 1;
            }

        })
        .fail(function (jqXHR, textStatus, errorThrown) {

            console.log('Failed : getDataFromServer');
        })


    },


    setupGrid: function (settings) {

        var canvas = $(this.config.canvas).get(0)
            config = this.config
        ;

        config.intervals = settings.interval;
        canvas.width = settings.maxWidth * config.grid.pixelSize;
        canvas.height = settings.maxHeight * config.grid.pixelSize;
    },

    renderGrid: function() {

        var canvas = $(this.config.canvas).get(0),
            context = canvas.getContext("2d"),
            grid = this.config.grid
        ;


        context.clearRect(0, 0, canvas.width, canvas.height);

        //context.save();
        context.lineWidth = grid.lineWidth;
        context.strokeStyle = grid.strokeColor;

        // X axis grid lines
        for(var x = 0; x <= canvas.height; x += grid.pixelSize)
        {
            context.beginPath();
            context.moveTo(0, x);
            context.lineTo(canvas.width, x);
            context.closePath();
            context.stroke();
        }

        // Y axis grid lines
        for(var y = 0; y <= canvas.width; y += grid.pixelSize)
        {
            context.beginPath();
            context.moveTo(y, 0);
            context.lineTo(y, canvas.height);
            context.closePath();
            context.stroke();
        }

        $.each(this.config.cells, function () {
           context.fillRect((this.X * grid.pixelSize), (this.Y  * grid.pixelSize), grid.pixelSize, grid.pixelSize);
        });

        //context.restore();
    }


};

$(document).ready(function ($) {

    GameOfLife.init();

});
