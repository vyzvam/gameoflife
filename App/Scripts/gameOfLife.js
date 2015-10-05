
/**
 * GameOfLife Literal that holds the variables and functions
 * @type {Object}
 */
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
            defaultFillColor: '#fff',
            lifeFillColor: '#000',
            zombieFillColor: '#f00'
        },
        cells : null
    },

    /**
     * Initializes the object with the required settings
     */
    init: function() {
        this.getGridSettings();
        this.bindEvents();

    },

    /**
     * Binds all the events required for the user interaction
     */
    bindEvents: function () {

        $doc = $(document);
        $doc.on('click', this.config.settingsForm.startButton, $.proxy(this.playLife, this));
        $doc.on('click', this.config.settingsForm.resetButton, this.resetLife);
    },

    /**
     * Gets the default settings of the grid from server and sets up the grid
     */
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

    /**
     * Plays / Pauses the simulation based on the event being triggered
     */
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

    /**
     * Resets the grid, or more appropriately reloads the site
     */
    resetLife: function () {
        location.reload();
    },

    /**
     * Gets the next generation data depending on the recursive condition
     */
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

    /**
     * Sets up the grid
     * @param  {object} settings grid settings
     */
    setupGrid: function (settings) {

        var canvas = $(this.config.canvas).get(0)
            config = this.config
        ;

        config.intervals = settings.interval;
        canvas.width = settings.maxWidth * config.grid.pixelSize;
        canvas.height = settings.maxHeight * config.grid.pixelSize;

        this.renderGrid();

    },

    /**
     * renders the grid on a canvas based on the processed data stored in config
     */
    renderGrid: function() {

        var self = this,
            canvas = $(this.config.canvas).get(0),
            context = canvas.getContext("2d"),
            grid = this.config.grid
        ;


        context.clearRect(0, 0, canvas.width, canvas.height);

        

        if (this.config.cells != null) {
            $.each(self.config.cells, function () {

                context.fillStyle = self.config.grid.defaultFillColor;

                if (this.Status == 1) {
                    context.fillStyle = self.config.grid.lifeFillColor;
                }
                if (this.Status == 3) {
                    context.fillStyle = self.config.grid.zombieFillColor;
                }

                context.fillRect((this.X * grid.pixelSize), (this.Y * grid.pixelSize), grid.pixelSize, grid.pixelSize);

            });
        }


        //context.save();
        context.lineWidth = grid.lineWidth;
        context.strokeStyle = grid.strokeColor;

        // X axis grid lines
        for (var x = 0; x <= canvas.height; x += grid.pixelSize) {
            context.beginPath();
            context.moveTo(0, x);
            context.lineTo(canvas.width, x);
            context.closePath();
            context.stroke();
        }

        // Y axis grid lines
        for (var y = 0; y <= canvas.width; y += grid.pixelSize) {
            context.beginPath();
            context.moveTo(y, 0);
            context.lineTo(y, canvas.height);
            context.closePath();
            context.stroke();
        }
        //context.restore();
    }


};

$(document).ready(function ($) {

    GameOfLife.init();

});
