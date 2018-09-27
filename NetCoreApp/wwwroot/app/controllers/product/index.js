var productController = function () {
    this.initialize = function () {
        getCategories();
        loadData();
        registerEvent();
        registerControl();
    };

    //Set value when choose dropdown list
    function registerEvent() {
        $('#frmMaintenance').validate({
            errorClass: 'red',
            ignore: [],
            rules: {
                txtNameM: { required: true },
                ddlCategoryIdM: { required: true },
                txtPriceM: {
                    required: true,
                    number: true
                },
                txtOriginalPriceM: {
                    required: true,
                    number: true
                }
            }
        });

        $('#ddlShowPage').on('change', function () {
            app.configs.pageSize = $(this).val();
            app.configs.pageIndex = 1;
            loadData(true);
        });
        $('#btnSearch').on('click', function () {
            loadData();
        });
        $('#txtKeyword').on('keypress',
            function (e) {
                if (e.which === 13) {
                    loadData();
                }
            });
        $('#btnCreate').off('click').on('click', function () {
            resetFormMaintenance();
            initTreeDropDownCategory();
            $('#modal-add-edit').modal('show');
        });


        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintenance').valid()) {
                e.preventDefault();
                var id = $('#hidIdM').val();
                var name = $('#txtNameM').val();
                var categoryId = $('#ddlCategoryIdM').combotree('getValue');
                var description = $('#txtDescM').val();
                var unit = $('#txtUnitM').val();
                var price = $('#txtPriceM').val();
                var originalPrice = $('#txtOriginalPriceM').val();
                var promotionPrice = $('#txtPromotionPriceM').val();
                //var image = $('#txtImageM').val();
                var tags = $('#txtTagM').val();
                var seoKeyword = $('#txtMetakeywordM').val();
                var seoMetaDescription = $('#txtMetaDescriptionM').val();
                var seoPageTitle = $('#txtSeoPageTitleM').val();
                var seoAlias = $('#txtSeoAliasM').val();
                var content = CKEDITOR.instances.txtContent.getData();
                var status = $('#ckStatusM').prop('checked') === true ? 1 : 0;
                var hot = $('#ckHotM').prop('checked');
                var showHome = $('#ckShowHomeM').prop('checked');
                $.ajax({
                    type: "POST",
                    url: "/Admin/Product/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        CategoryId: categoryId,
                        Image: '',
                        Price: price,
                        OriginalPrice: originalPrice,
                        PromotionPrice: promotionPrice,
                        Description: description,
                        Content: content,
                        HomeFlag: showHome,
                        HotFlag: hot,
                        Tags: tags,
                        Unit: unit,
                        Status: status,
                        SeoPageTitle: seoPageTitle,
                        SeoAlias: seoAlias,
                        SeoKeywords: seoKeyword,
                        SeoDescription: seoMetaDescription
                    },
                    dataType: "json",
                    beforeSend: function () {
                        app.startLoading();
                    },
                    success: function (response) {
                        app.notify('Update product successful', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintenance();
                        app.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        app.notify('Has an error in save product progress', 'error');
                        app.stopLoading();
                    }
                });
                return false;
            }
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: 'GET',
                data: {
                    id: that
                },
                dataType: 'json',
                url: '/Admin/Product/GetProductById',
                beforeSend: function () {
                    app.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidIdM').val(data.Id);
                    $('#txtNameM').val(data.Name);
                    initTreeDropDownCategory(data.CategoryId);
                    $('#txtDescM').val(data.Description);
                    $('#txtUnitM').val(data.Unit);
                    $('#txtPriceM').val(data.Price);
                    $('#txtOriginalPriceM').val(data.OriginalPrice);
                    $('#txtPromotionPriceM').val(data.PromotionPrice);
                    // $('#txtImageM').val(data.ThumbnailImage);
                    $('#txtTagM').val(data.Tags);
                    $('#txtMetakeywordM').val(data.SeoKeywords);
                    $('#txtMetaDescriptionM').val(data.SeoDescription);
                    $('#txtSeoPageTitleM').val(data.SeoPageTitle);
                    $('#txtSeoAliasM').val(data.SeoAlias);
                    CKEDITOR.instances.txtContent.setData(data.Content);
                    $('#ckStatusM').prop('checked', data.Status === 1);
                    $('#ckHotM').prop('checked', data.HotFlag);
                    $('#ckShowHomeM').prop('checked', data.HomeFlag);
                    $('#modal-add-edit').modal('show');
                    app.stopLoading();
                },
                error: function (error) {
                    app.notify('Has an Error', 'error');
                    app.stopLoading();
                }
            });
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            app.confirm('Are you sure', function () {
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Product/Delete',
                    dataType: 'json',
                    data: {
                        id: that
                    },
                    beforeSend: function () {
                        app.startLoading();
                    },
                    success: function (response) {
                        app.notify('Delete success', 'success');
                        app.stopLoading();
                        //location.reload();
                        loadData(true);
                    },
                    error: function () {
                        app.notify('Has an error', 'error');
                        app.stopLoading();
                    }
                });
            });
        });
    }

    function registerControl() {
        CKEDITOR.replace('txtContent', {});
        //Fix: cannot click on element ck in modal
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            $(document)
                .off('focusin.bs.modal') // guard against infinite focus loop
                .on('focusin.bs.modal',
                    $.proxy(function (e) {
                        if (
                            this.$element[0] !== e.target &&
                            !this.$element.has(e.target).length
                            // CKEditor compatibility fix start.
                            &&
                            !$(e.target).closest('.cke_dialog, .cke').length
                            // CKEditor compatibility fix end.
                        ) {
                            this.$element.trigger('focus');
                        }
                    },
                        this));
        };
    }

    function getCategories() {
        var render = "<option value=''>--Select Category--</option>";
        $.ajax({
            type: 'GET',
            url: '/admin/product/GetCategories',
            data: 'json',
            success: function (response) {
                if (response.length === 0) {
                    app.notify('No Data', 'success');
                } else {
                    $.each(response,
                        function (i, item) {
                            render += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                        });
                    $('#ddlCategorySearch').html(render);
                }
            },
            error: function (error) {
                console.log(error);
                app.notify(error, 'error');
            }
        });
    }

    function loadData(isPageChanged) {
        var template = $('#table-template-product').html();
        var render = "";
        $.ajax({
            type: 'GET',
            url: '/admin/product/GetAllPaging',
            data: {
                categoryId: $('#ddlCategorySearch').val(),
                keyword: $('#txtKeyword').val(),
                page: app.configs.pageIndex,
                pageSize: app.configs.pageSize
            },
            dataType: 'json',
            success: function (response) { 
                if (response.Results.length === 0) {
                    app.notify('No data', 'warn');
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
                                    CreateDay: app.dataTimeFormatJson(item.DateCreated),
                                    UpdateDay: app.dataTimeFormatJson(item.DateModified),
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

    function resetFormMaintenance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        initTreeDropDownCategory('');
        $('#txtDescM').val('');
        $('#txtUnitM').val('');
        $('#txtPriceM').val('0');
        $('#txtOriginalPriceM').val('0');
        $('#txtPromotionPriceM').val('0');
        //$('#txtImageM').val('');
        $('#txtTagM').val('');
        $('#txtMetakeywordM').val('');
        $('#txtMetaDescriptionM').val('');
        $('#txtSeoPageTitleM').val('');
        $('#txtSeoAliasM').val('');
        //CKEDITOR.instances.txtContentM.setData('');
        $('#ckStatusM').prop('checked', true);
        $('#ckHotM').prop('checked', false);
        $('#ckShowHomeM').prop('checked', false);
    }

    function initTreeDropDownCategory(selectedId) {
        $.ajax({
            url: "/Admin/ProductCategory/GetProductCategory",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });
                var arr = app.unflattened(data);
                $('#ddlCategoryIdM').combotree({
                    data: arr
                });
                if (selectedId !== undefined) {
                    $('#ddlCategoryIdM').combotree('setValue', selectedId);
                }
            }
        });
    }
};

