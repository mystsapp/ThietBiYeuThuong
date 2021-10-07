function addCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

var tb_indexController = {
    init: function () {
        toastr.options = { // toastr options
            "closeButton": false,
            "debug": false,
            "newestOnTop": false,
            "progressBar": true,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "2000",
            "timeOut": "2000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };

        tb_indexController.registerEvent();
    },

    registerEvent: function () {
        // format .numbers
        $('input.numbers').keyup(function (event) {
            // Chỉ cho nhập số
            if (event.which >= 37 && event.which <= 40) return;

            $(this).val(function (index, value) {
                return addCommas(value);
            });
        });

        //// phieu click --> load kvctpct
        //$('tr .tdVal').click(function () {
        //    id = $(this).data('id');

        //    tb_indexController.TdVal_LoaiThietBi_Click(id);
        //});
        //// phieu click --> load kvctpct

        // giu trang thai phieu click
        $('#loaiThietBiTbl .cursor-pointer').off('click').on('click', function () {
            if ($(this).hasClass("hoverClass"))
                $(this).removeClass("hoverClass");
            else {
                $('.cursor-pointer').removeClass("hoverClass");
                $(this).addClass("hoverClass");
            }
        });
        // giu trang thai phieu click

        // giu trang thai CT phieu click
        $('#cTPhieuTbl .ctphieu-cursor-pointer').off('click').on('click', function () {
            if ($(this).hasClass("hoverClass"))
                $(this).removeClass("hoverClass");
            else {
                $('.ctphieu-cursor-pointer').removeClass("hoverClass");
                $(this).addClass("hoverClass");
            }
        });
        // giu trang thai CT phieu click

        // btnInPhieu
        $('#btnInPhieu').off('click').on('click', function () {
            $('#frmInPhieu').submit();
        })
    },
    Load_ThietBiPartial: function (id, page) { // PhieuNX id
        var url = '/TinhTrangBN/TinhTrangBNPartial';
        $.get(url, { id: id, page: page }, function (response) {
            $('#TinhTrangBN_Tbl').html(response);
            $('#TinhTrangBN_Tbl').show(500);
        });
    },
    TdVal_LoaiThietBi_Click: function (id) { // PhieuNX id
        // page
        var page = $('.active span').text();
        // $('#hidPage').val(page);

        //// gang' loaiphieu
        //$('#hidLoaiPhieu').val(loaiPhieu);
        ////$('#hidLoaiPhieuFull').text(loaiPhieu);

        //$('#KVCTPCT_Create_Partial').hide(500);
        //$('#KVCTPCT_Edit_Partial').hide(500);

        tb_indexController.Load_ThietBiPartial(id, page); // KVPTC id
    }
};
tb_indexController.init();