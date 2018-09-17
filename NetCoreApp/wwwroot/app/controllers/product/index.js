var productController = function () {
    this.initialize = function () {
        loadData();
        registerEvent();
    };

    //Set value when choose dropdown list
    function registerEvent() {
        $('#ddlShowPage').on('change', function () {
            app.configs.pageSize = $(this).val();
            app.configs.pageIndex = 1;
            loadData(true);
        });
    }

    function loadData(isPageChanged) {
        var template = $('#table-template-product').html();
        var render = "";
        $.ajax({
            type: 'GET',
            url: '/admin/product/GetAllPaging',
            data: {
                categoryId: null,
                keyword: $('#txtKeyword').val(),
                page: app.configs.pageIndex,
                pageSize: app.configs.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.length == 0) {
                    app.notify('No data', 'error');
                } else {
                    $.each(response.Results,
                        function (i, item) {
                            //Using Mustache to render data
                            render += Mustache.render(template,
                                {
                                    Id: item.Id,
                                    Name: item.Name,
                                    Category: item.ProductCategory.Name,
                                    Price: item.Price,
                                    Image: item.Image = '<img src="/vendor/gentelella/production/user.png" width = "25px"></img>',
                                    //? '<img src="' + item.Image + '"width="25px"></img>'
                                    //: '<img src="~/vendor/gentelella/production/user.png"></img>',
                                    CreateDay: app.dateFormatJson(item.DateCreated),
                                    Status: app.getStatus(item.Status)
                                });
                            if (render !== "") {
                                $('#lblTotalRecords').text(response.RowCount);
                                $('#table-content').html(render);
                            }
                            wrapPaging(response.RowCount, function () {
                                loadData();
                            }, isPageChanged);
                        });
                }

            },
            error: function (error) {
                console.log(error);
                app.notify('Can not Load Product', 'error');
            }
        });
    }

    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalSize = Math.ceil(recordCount / app.configs.pageSize);
        //Unbind pagination if it existed or click change pageSize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalSize,
            visiblePages: 7,
            first: 'First',
            prev: 'Previous',
            next: 'Next',
            last: 'Last',
            onPageClick: function (event, p) {
                //Assign if p!==CurrentPage
                if (app.configs.pageIndex !== p) {
                    app.configs.pageIndex = p;
                    setTimeout(callBack(), 200);
                }
            }
        });
    }
};

