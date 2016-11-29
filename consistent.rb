# * which of the num fields is greater?
#   * tie - choose leftmost
# * which of the last gift dates is most recent?
#   * tie - choose leftmost

require "csv"
require "time"

# Email,First,Last,SPDate,RRDate,DMDate,SPNum,RRNum,DMNum,SP,RR,DM

# begin

  CSV.open("constent-new.csv", "wb") do |csv|
    CSV.foreach("consistent.csv", headers: true, converters: :numeric) do |row|
      begin
        gifts = []
        gifts << row["SPNum"] unless row["SPNum"].nil?
        gifts << row["RRNum"] unless row["RRNum"].nil?
        gifts << row["DMNum"] unless row["DMNum"].nil?

        if gifts.empty?
          dates = []
          dates << Date.strptime(row["SPDate"], "%m/%d/%y") unless row["SPDate"].nil?
          dates << Date.strptime(row["RRDate"], "%m/%d/%y") unless row["RRDate"].nil?
          dates << Date.strptime(row["DMDate"], "%m/%d/%y") unless row["DMDate"].nil?

          if dates.empty?
            puts row.inspect
          else
            max = dates.max

            row["SP"] = "SP" if row["SPDate"] != nil && Date.strptime(row["SPDate"], "%m/%d/%y") == max
            row["SP"] = "RR" if row["RRDate"] != nil && Date.strptime(row["RRDate"], "%m/%d/%y") == max
            row["SP"] = "DM" if row["DMDate"] != nil && Date.strptime(row["DMDate"], "%m/%d/%y") == max
          end
        else
          max = gifts.max

          row["SP"] = "SP" if row["SPNum"] == max
          row["SP"] = "RR" if row["RRNum"] == max
          row["SP"] = "DM" if row["DMNum"] == max
        end

        csv << row
      rescue Exception => e
        puts e.message
        puts row.inspect
        exit
      end
    end
  end

# rescue Exception => e
#   puts row.inspect
# end
