var GameOfLife = {
    config: {
        settingsForm: {
            id: '#settings-form',
            startButton: '#play-button',
            resetButton: '#reset-button',
            cells: '#form-cells'
        },
        canvas: '#canvas-test',
        intervalCounter: 0,
        intervals: 2,
        isToStop: false,
        grid: {
            pixelSize: 10,
            lineWidth: 0.3,
            strokeColor: '#aaa',
            lifeFillColor: '#fff',
            zombieFillColor: '#f00'
        },
        cells : null
    },

    init: function() {
        this.getGridSettings();
        this.bindEvents();

    },

    bindEvents: function () {

        $doc = $(document);
        $doc.on('click', this.config.settingsForm.startButton, $.proxy(this.playLife, this));
        $doc.on('click', this.config.settingsForm.resetButton, this.resetLife);
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

    playLife: function () {

        var $button = $(this.config.settingsForm.startButton);


        if ($button.data('state') == '0') {
            $button.data('state', '1');
            $button.text('Stop!');
            this.config.isToStop = false;
            this.getDataFromServer();
        }
        else {
            $button.data('state', '0');
            $button.text('Start!');
            this.config.isToStop = true;
        }

    },

    resetLife: function () {
        location.reload();
    },

    getDataFromServer: function () {
        var self = this,
            form = self.config.settingsForm.id,
            cells = self.config.settingsForm.cells
        ;

        $(cells).val(JSON.stringify(self.config.cells));

        $.ajax(
            {
                type: 'POST',
                url: 'Home/GetGenerationData',
                data: $(form).serialize()
            }
        )
        .done(function (response, textStatus, jqXHR) {

            self.config.cells = response;
            self.renderGrid();

            if (!self.config.isToStop) {
                if (self.config.intervalCounter < self.config.intervals) {
                    self.getDataFromServer();
                    self.config.intervalCounter += 1;
                }
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

        this.renderGrid();

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

        if (this.config.cells != null) {
            $.each(this.config.cells, function () {
                context.fillRect((this.X * grid.pixelSize), (this.Y * grid.pixelSize), grid.pixelSize, grid.pixelSize);
            });
        }

        //context.restore();
    }


};

$(document).ready(function ($) {

    GameOfLife.init();

});
