@extends('layouts.app')

@section('content')
<div class="container">
    <div class="row justify-content-center">
        <div class="col-md-8">
            <form class="analysis-form" action="{{ route('analysis.store') }}" method="POST">
                @csrf
                <div class="d-flex justify-content-center align-items-center gap-3">
                    <div>
                        <label for="dateBegin" class="form-label">Дата начала</label>
                    </div>

                    <div>
                        <input class="form-control" type="date" name="dateBegin"
                               @isset($params)value="{{ $params['dateBegin'] }}" @endisset id="dateBegin" required>
                    </div>

                    <div>
                        <label for="dateEnd" class="form-label">Дата окончания</label>
                    </div>
                    <div>
                        <input class="form-control" type="date" name="dateEnd"
                               @isset($params)value="{{ $params['dateEnd'] }}" @endisset id="dateEnd" required>
                    </div>
                    <div>
                        <button class="btn btn-primary" type="submit">Показать</button>
                    </div>
                </div>

            </form>
        </div>
    </div>
</div>

<div class="p-3">
    <div class="row justify-content-center">

        @isset($response)


        <link href="https://cdn.datatables.net/v/bs5/jq-3.6.0/jszip-2.5.0/dt-1.13.4/b-2.3.6/b-colvis-2.3.6/b-html5-2.3.6/b-print-2.3.6/date-1.4.1/fc-4.2.2/fh-3.3.2/r-2.4.1/rg-1.3.1/rr-1.3.3/sc-2.1.1/sb-1.4.2/sp-2.1.2/sl-1.6.2/datatables.min.css"
              rel="stylesheet"/>

        <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.7/pdfmake.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.7/vfs_fonts.js"></script>
        <script src="https://cdn.datatables.net/v/bs5/jq-3.6.0/jszip-2.5.0/dt-1.13.4/b-2.3.6/b-colvis-2.3.6/b-html5-2.3.6/b-print-2.3.6/date-1.4.1/fc-4.2.2/fh-3.3.2/r-2.4.1/rg-1.3.1/rr-1.3.3/sc-2.1.1/sb-1.4.2/sp-2.1.2/sl-1.6.2/datatables.min.js"></script>


        <table id="example" class="table table-striped" style="width:100%">
            <thead>
            <tr>
                <!--                    <th>Идентификатор записи</th>-->
                <th>Пол пациента</th>
                <th>Дата рождения пациента</th>
                <th nowrap>ID пациента</th>
                <th nowrap>Код МКБ-10</th>
                <th>Диагноз</th>
                <th nowrap>Дата оказания услуги</th>
                <th>Должность</th>
                <th>Назначения</th>
                <th nowrap>Стандарт помощи</th>
                <th>Статус</th>
                <!--                    <th>Дата и время загрузки данных в БД</th>-->
            </tr>
            </thead>
            <tbody>

            @foreach($response as $responseData)
            <tr>
                <!--                    <td>{{ $responseData['id'] }}</td>-->
                <td>{{ $responseData['sex'] }}</td>
                <td>{{ \Carbon\Carbon::parse($responseData['birthdayDate'])->format('d.m.Y') }}</td>
                <td>{{ $responseData['clientId'] }}</td>
                <td>{{ $responseData['mkb'] }}</td>
                <td>{{ $responseData['diagnosis'] }}</td>
                <td>{{ \Carbon\Carbon::parse($responseData['treatDate'])->format('d.m.Y') }}</td>
                <td>{{ $responseData['doctorPosition'] }}</td>
                <td><a href="#" data-bs-toggle="modal" data-bs-target="#modal_{{ $responseData['id'] }}">{{count($responseData['referrals'])}}
                        шт.</a>

                </td>
                <td class="{{ $responseData['treatmentHasStandards'] ? 'text-success' : 'text-danger' }} ">{{
                    $responseData['treatmentHasStandards'] ? 'Есть' : 'Нет' }}
                </td>
                <td class="@if($responseData['referralStatus'] == 'Все назначения соответствуют стандартам') {{ 'bg-success' }}  @elseif($responseData['referralStatus'] == 'Назначения частично соответствуют стандартам') {{ 'bg-info' }}  @elseif($responseData['referralStatus'] == 'Все назначения не соответствуют стандартам') {{ 'bg-danger' }}  @endif">
                    {{ $responseData['referralStatus'] }}
                </td>

                <!--                    <td>{{ $responseData['createDateTime'] }}</td>-->
            </tr>
            @endforeach


            </tbody>
        </table>

        @foreach($response as $responseData)
        <!-- Modal -->
        <div class="modal fade" id="modal_{{ $responseData['id'] }}" tabindex="-1" aria-labelledby="exampleModalLabel"
             aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h1 class="modal-title fs-5" id="exampleModalLabel">Назначения</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body ">
                        <table class="table">
                            @foreach($responseData['referrals'] as $referral)
                            <tr>
                                <td class="fw-bold">{{ $referral['name'] }}</td>
                                <td>
                                    <table class="table table-striped table-sm" style="width: 100%">
                                    <tr><td>Присутствует в стандартах:</td><td>{{ $referral['isPresentsInStandards'] ? 'Да' : 'Нет' }}</td></tr>
                                        @if($referral['isPresentsInStandards'])
                                    <tr><td>Тип стандарта: </td><td>{{ $referral['associatedStandard'] }}</td></tr>
                                    <tr><td>Наименование стандарта:</td><td> {{ $referral['associatedStandardsName'] }}</td></tr>
                                    <tr><td>Наименование услуги из стандарта:</td><td>{{ $referral['associatedServiceName'] }}</td></tr>
                                    <tr><td>Обязательная услуга: </td><td>{{ $referral['isServiceNecessary'] ? 'Да' : 'Нет' }}</td></tr>@endif
                                    </table>
                                </td>
                            </tr>
                            @endforeach
                        </table>
                    </div>

                </div>
            </div>
        </div>
        @endforeach

        <script>
            $(document).ready(function () {
                var table = $('#example').DataTable({
                    // "columnDefs": [
                    //     { "visible": false, "targets": [0, 9] }
                    // ],
                    scrollX: true,
                    "language": {
                        "lengthMenu": "Показывать _MENU_ записей",
                        "search": "Поиск:",
                        "zeroRecords": "нет записей",
                        "paginate": {
                            "first": "Первый",
                            "last": "Последний",
                            "next": "Вперед",
                            "previous": "Назад"
                        },
                        "info": "Показ _START_ по _END_ из _TOTAL_ записей",
                        "infoEmpty": "",
                    },
                    //scrollX: true,
                    dom: 'Bfrtip',
                    buttons: [
                        {
                            extend: 'colvis',
                            text: 'Скрыть колонки',
                            header: false
                        },
                        // 'columnsToggle',
                        'spacer',
                        {
                            extend: 'pdfHtml5',
                            //text: 'Save current page',
                            exportOptions: {
                                columns: ':visible',
                            },
                            header: false
                        },
                        {
                            extend: 'excelHtml5',
                            //text: 'Save current page',
                            exportOptions: {
                                columns: ':visible',
                            },
                            header: false
                        },
                        {
                            extend: 'print',
                            text: 'Печать',
                            exportOptions: {
                                columns: ':visible',
                            },
                            header: false
                        },


                    ],
                    initComplete: function () {
                        this.api()
                            .columns()
                            .every(function () {
                                var column = this;
                                var select = $('<br><select class="filter-data"><option value=""></option></select>')
                                    .appendTo($(column.header()))
                                    .on('change', function () {
                                        var val = $.fn.dataTable.util.escapeRegex($(this).val());

                                        column.search(val ? '^' + val + '$' : '', true, false).draw();
                                    });

                                column
                                    .data()
                                    .unique()
                                    .sort()
                                    .each(function (d, j) {
                                        select.append('<option value="' + d + '">' + d + '</option>');
                                    });
                            });
                    },
                });


            });
        </script>

        @endisset

    </div>
</div>
@endsection
